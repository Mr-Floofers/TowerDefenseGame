using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense
{
    class Unit : Sprite
    {
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        };

        public enum UnitStates
        {
            ReachedTheEnd,
            Dead,
            Alive
        };

        //Vector2 onScreenPosition;
        int pathIndex;
        TimeSpan moveTimer;

        /////
        float currentAngle;
        bool newAngle;
        /////


        Vector2 gridPosition => TowerDefense.Grid[onScreenPosition].GridPosition; //make it so that this is calculated from onScreenPosition
        float movementSpeed; // pixels per second
        //Texture2D texture; //?


        Direction currentDirection;
        public UnitStates UnitState { get; private set; }

        Vector2 debugMousePosition;
        Vector2 debugCenter;

        public Unit(float speed, Texture2D texture)
            : base(texture)
        {
            movementSpeed = speed;
            //this.texture = texture;
            moveTimer = TimeSpan.Zero;
            pathIndex = 0;
            onScreenPosition = TowerDefense.Grid[pathIndex] + new Vector2(TowerDefense.Grid.SquareSize / 2);
            debugMousePosition = Vector2.Zero;
            newAngle = true;
            debugCenter = Vector2.Zero;


            origin = Vector2.One / 2f;
            scale = Vector2.One * 20;
            layerDepth = 0f;
            color = Color.Red;
        }

        public void Update(GameTime gameTime)//true = done
        {
            Vector2 targetOnScreenPosition = TowerDefense.Grid[pathIndex + 1] + new Vector2(TowerDefense.Grid.SquareSize / 2);//TowerDefense.Grid.Map.Path[pathIndex+1]; // TODO: Get next target
            //var thing = TowerDefense.Grid[targetOnScreenPosition];
            Vector2 gridSquarePosotionToBaseMovementOn = TowerDefense.Grid[pathIndex];// + new Vector2(TowerDefense.Grid.SquareSize / 2);
            GridSquare gridSquareToBaseMovementOn = TowerDefense.Grid[onScreenPosition];//onScreenPosition

            if (currentDirection == Direction.Down)
            {
                color = Color.Blue;
            }
            if (currentDirection == Direction.Right)
            {
                color = Color.Red;
            }
            if (currentDirection == Direction.Left)
            {
                color = Color.OliveDrab;
            }
            if (currentDirection == Direction.Up)
            {
                color = Color.Black;
            }

            if (gridSquareToBaseMovementOn == null && pathIndex < 1)
            {
                gridSquareToBaseMovementOn = TowerDefense.Grid[targetOnScreenPosition];
                currentDirection = directionHelper2Vectors(onScreenPosition, targetOnScreenPosition);
                //var directionVector = gridSquareToBaseMovementOn.onScreenPosition - 
            }
            if (gridSquareToBaseMovementOn == null)
            {
                //throw new Exception("u suck");
                UnitState = UnitStates.ReachedTheEnd;
                return;
            }

            //Debug.WriteLine(pathIndex);

            switch (gridSquareToBaseMovementOn.TileKind)
            {
                case Grid.TileKinds.Horizontal:
                case Grid.TileKinds.HorizontalDrawLow:
                case Grid.TileKinds.HorizontalDrawHigh:
                case Grid.TileKinds.Vertical:
                case Grid.TileKinds.VerticalDrawLeft:
                case Grid.TileKinds.VerticalDrawRight:
                    {
                        var moveAmount = (float)(movementSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                        var moveDirection = targetOnScreenPosition - onScreenPosition;
                        moveDirection.Normalize();

                        var moveVector = moveDirection * moveAmount;
                        onScreenPosition += moveVector;
                        Vector2 test = onScreenPosition - targetOnScreenPosition;
                        //debugCenter = targetOnScreenPosition;
                        if (Math.Abs(test.X) < 1f && Math.Abs(test.Y) < 1f && pathIndex + 2 < TowerDefense.Grid.Map.Path.Count)
                        {
                            pathIndex++;
                        }

                        currentDirection = directionHelper1Vector(moveDirection);
                    }
                    break;

                case Grid.TileKinds.TurnFromEntryLeftToUpperRight:
                    {
                        if (currentDirection == Direction.Right)
                        {
                            if (newAngle)
                            {
                                currentAngle = 90;
                                newAngle = false;
                            }
                            float distanceToTravel = .5f * (float)Math.PI * TowerDefense.Grid.SquareSize / 2;
                            float thyme = distanceToTravel / movementSpeed;
                            float angularVelocity = 90 / thyme;

                            float targetAngle = 0;
                            Vector2 centerOfRotation = new Vector2(gridPosition.X, gridPosition.Y) * TowerDefense.Grid.SquareSize;

                            float radius = TowerDefense.Grid.SquareSize / 2;

                            debugCenter = centerOfRotation;

                            targetOnScreenPosition = RotateWalk(radius, currentAngle - 1, centerOfRotation);

                            onScreenPosition = targetOnScreenPosition;


                            currentAngle -= angularVelocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);

                            if (currentAngle <= targetAngle)
                            {
                                pathIndex++;

                                onScreenPosition.Y = (float)Math.Floor(onScreenPosition.Y) - 1;
                                //onScreenPosition.X = (float)Math.Ceiling(onScreenPosition.X);
                                newAngle = true;
                                currentDirection = Direction.Up;
                            }
                        }
                        else if(currentDirection == Direction.Down)
                        {
                            if (newAngle)
                            {
                                currentAngle = 0;
                                newAngle = false;
                            }

                            float targetAngle = 90;
                            Vector2 centerOfRotation = new Vector2(gridPosition.X, gridPosition.Y) * TowerDefense.Grid.SquareSize;

                            var posAndAngle = CurveMove(currentAngle, targetAngle, movementSpeed, centerOfRotation, gameTime);

                            debugCenter = centerOfRotation;

                            onScreenPosition = posAndAngle.Position;
                            currentAngle = posAndAngle.Angle;

                            if (currentAngle >= targetAngle)
                            {
                                pathIndex++;

                                // Round up, because floats
                                onScreenPosition.Y = (float)Math.Ceiling(onScreenPosition.Y);
                                onScreenPosition.X = (float)Math.Floor(onScreenPosition.X)-1;

                                newAngle = true;
                                currentDirection = Direction.Left;
                            }
                        }
                    }
                    break;
                case Grid.TileKinds.TurnFromEntryBelowToUpperLeft:
                    {
                        if (currentDirection == Direction.Right)
                        {
                            if (newAngle)
                            {
                                currentAngle = 270;
                                newAngle = false;
                            }

                            float targetAngle = 360;
                            Vector2 centerOfRotation = new Vector2(gridPosition.X, gridPosition.Y + 1) * TowerDefense.Grid.SquareSize;

                            var posAndAngle = CurveMove(currentAngle, targetAngle, movementSpeed, centerOfRotation, gameTime);

                            debugCenter = centerOfRotation;

                            onScreenPosition = posAndAngle.Position;
                            currentAngle = posAndAngle.Angle;

                            if (currentAngle >= targetAngle)
                            {
                                pathIndex++;

                                // Round up, because floats
                                onScreenPosition.Y = (float)Math.Ceiling(onScreenPosition.Y);
                                onScreenPosition.X = (float)Math.Ceiling(onScreenPosition.X);

                                newAngle = true;
                                currentDirection = Direction.Down;
                            }
                        }
                        else if (currentDirection == Direction.Up)
                        {
                            if (newAngle)
                            {
                                currentAngle = 270;
                                newAngle = false;
                            }

                            float targetAngle = 180;
                            Vector2 centerOfRotation = new Vector2(gridPosition.X, gridPosition.Y + 1) * TowerDefense.Grid.SquareSize;

                            var posAndAngle = CurveMove(currentAngle, targetAngle, movementSpeed, centerOfRotation, gameTime);

                            debugCenter = centerOfRotation;

                            onScreenPosition = posAndAngle.Position;
                            currentAngle = posAndAngle.Angle;

                            if (currentAngle <= targetAngle)
                            {
                                pathIndex++;

                                // Round up, because floats
                                onScreenPosition.Y = (float)Math.Ceiling(onScreenPosition.Y);
                                onScreenPosition.X = (float)Math.Floor(onScreenPosition.X)-1;

                                newAngle = true;
                                currentDirection = Direction.Left;
                            }
                        }
                    }
                    break;
                case Grid.TileKinds.TurnFromEntryRightToLowerLeft:
                    {
                        if (currentDirection == Direction.Up)
                        {
                            if (newAngle)
                            {
                                currentAngle = 180;
                                newAngle = false;
                            }
                            float distanceToTravel = .5f * (float)Math.PI * TowerDefense.Grid.SquareSize / 2;
                            float thyme = distanceToTravel / movementSpeed;
                            float angularVelocity = 90 / thyme;

                            float targetAngle = 270;
                            Vector2 centerOfRotation = new Vector2(gridPosition.X + 1, gridPosition.Y + 1) * TowerDefense.Grid.SquareSize;

                            float radius = TowerDefense.Grid.SquareSize / 2;

                            debugCenter = centerOfRotation;

                            targetOnScreenPosition = RotateWalk(radius, currentAngle + 1, centerOfRotation);

                            onScreenPosition = targetOnScreenPosition;


                            currentAngle += angularVelocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);

                            if (currentAngle >= targetAngle)
                            {
                                pathIndex++;

                                //onScreenPosition.Y = (float)Math.Floor(onScreenPosition.Y) - 1;
                                onScreenPosition.X = (float)Math.Ceiling(onScreenPosition.X);
                                newAngle = true;
                                currentDirection = Direction.Right;
                            }
                        }
                        else if(currentDirection == Direction.Left)
                        {
                            if (newAngle)
                            {
                                currentAngle = 270;
                                newAngle = false;
                            }

                            float targetAngle = 180;
                            Vector2 centerOfRotation = new Vector2(gridPosition.X+1, gridPosition.Y + 1) * TowerDefense.Grid.SquareSize;

                            var posAndAngle = CurveMove(currentAngle, targetAngle, movementSpeed, centerOfRotation, gameTime);

                            debugCenter = centerOfRotation;

                            onScreenPosition = posAndAngle.Position;
                            currentAngle = posAndAngle.Angle;

                            if (currentAngle <= targetAngle)
                            {
                                pathIndex++;

                                // Round up, because floats
                                onScreenPosition.Y = (float)Math.Ceiling(onScreenPosition.Y);
                                onScreenPosition.X = (float)Math.Ceiling(onScreenPosition.X);

                                newAngle = true;
                                currentDirection = Direction.Down;
                            }
                        }
                    }

                    break;
                case Grid.TileKinds.TurnFromEntryAboveToLowerRight:
                    {
                        if (currentDirection == Direction.Down)
                        {
                            if (newAngle)
                            {
                                currentAngle = 180;
                                newAngle = false;
                            }

                            float targetAngle = 90;
                            Vector2 centerOfRotation = new Vector2(gridPosition.X + 1, gridPosition.Y) * TowerDefense.Grid.SquareSize;

                            var posAndAngle = CurveMove(currentAngle, targetAngle, movementSpeed, centerOfRotation, gameTime);

                            onScreenPosition = posAndAngle.Position;
                            currentAngle = posAndAngle.Angle;

                            if (currentAngle <= targetAngle)
                            {
                                pathIndex++;

                                // Round up, because floats
                                onScreenPosition.Y = (float)Math.Ceiling(onScreenPosition.Y);
                                onScreenPosition.X = (float)Math.Ceiling(onScreenPosition.X);

                                currentDirection = Direction.Right;
                                newAngle = true;
                            }
                        }
                        else if(currentDirection == Direction.Left)
                        {
                            if (newAngle)
                            {
                                currentAngle = 180;
                                newAngle = false;
                            }

                            float targetAngle = 90;
                            Vector2 centerOfRotation = new Vector2(gridPosition.X, gridPosition.Y) * TowerDefense.Grid.SquareSize;

                            var posAndAngle = CurveMove(currentAngle, targetAngle, movementSpeed, centerOfRotation, gameTime);

                            debugCenter = centerOfRotation;

                            onScreenPosition = posAndAngle.Position;
                            currentAngle = posAndAngle.Angle;

                            if (currentAngle <= targetAngle)
                            {
                                pathIndex++;

                                // Round up, because floats
                                onScreenPosition.Y = (float)Math.Ceiling(onScreenPosition.Y);
                                onScreenPosition.X = (float)Math.Floor(onScreenPosition.X)-1;

                                newAngle = true;
                                currentDirection = Direction.Left;
                            }
                        }
                    }
                    break;

                case Grid.TileKinds.None:
                default:
                    break;
            }

            var mouseState = Mouse.GetState();
            debugMousePosition = mouseState.Position.ToVector2();

            UnitState = UnitStates.Alive;

            #region helpingStuff

            static Direction directionHelper2Vectors(Vector2 start, Vector2 end)
            {
                //mod itself+1
                int x = 0;
                //x %= x + 1;//this doent work if going right to left
                //x -= x-1;
                int y = 0;

                if (start.X - end.X < 0)
                {
                    return Direction.Right;
                    x = 1;
                }
                else if (start.X - end.X > 0)
                {
                    return Direction.Left;
                    x = -1;
                }
                if (start.Y - end.Y < 0)
                {
                    return Direction.Down;
                    y = 1;
                }
                else if (start.Y - end.Y > 0)
                {
                    return Direction.Up;
                    y = -1;
                }

                return Direction.Down;
            }

            static Direction directionHelper1Vector(Vector2 pos)
            {
                //mod itself+1
                int x = 0;
                //x %= x + 1;//this doent work if going right to left
                //x -= x-1;
                int y = 0;

                if (pos.X < 0)
                {
                    return Direction.Left;
                    x = 1;
                }
                else if (pos.X > 0)
                {
                    return Direction.Right;
                    x = -1;
                }
                if (pos.Y < 0)
                {
                    return Direction.Up;
                    y = 1;
                }
                else if (pos.Y > 0)
                {
                    return Direction.Down;
                    y = -1;
                }

                return Direction.Down;
            }

            static (Vector2 Position, float Angle) CurveMove(float currentAngle, float targetAngle, float movementSpeed, Vector2 centerOfRotation, GameTime gameTime)
            {
                float distanceToTravel = .5f * (float)Math.PI * TowerDefense.Grid.SquareSize / 2;
                float thyme = distanceToTravel / movementSpeed;
                float angularVelocity = 90 / thyme;

                float radius = TowerDefense.Grid.SquareSize / 2;

                if (currentAngle > targetAngle)
                {
                    return (RotateWalk(radius, currentAngle - 1, centerOfRotation), currentAngle - angularVelocity * (float)(gameTime.ElapsedGameTime.TotalSeconds));
                }
                else
                {
                    return (RotateWalk(radius, currentAngle + 1, centerOfRotation), currentAngle + angularVelocity * (float)(gameTime.ElapsedGameTime.TotalSeconds));
                }
            }

            static Vector2 RotateWalk(float radius, float angle, Vector2 center)
            {
                return new Vector2((radius * (float)Math.Cos(angle * Math.PI / 180)) + center.X, (radius * (float)Math.Sin(angle * Math.PI / 180)) + center.Y);
            }

            #endregion helpingStuff
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            sb.Draw(GridBasedGame.Pixel, debugCenter, null, Color.Green, 0f, Vector2.Zero, 20f, SpriteEffects.None, 0f );
        }
    }
}
