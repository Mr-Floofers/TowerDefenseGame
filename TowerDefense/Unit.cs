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
    class Unit
    {
        Vector2 onScreenPosition;
        int pathIndex;
        TimeSpan moveTimer;

        /////
        float currentAngle;
        bool newAngle;
        /////


        Vector2 gridPosition => TowerDefense.Grid[onScreenPosition].GridPosition; //make it so that this is calculated from onScreenPosition
        float movementSpeed; // pixels per second
        Texture2D texture; //?


        Vector2 debugMousePosition;
        Vector2 debugCenter;

        public Unit(float speed, Texture2D texture)
        {
            movementSpeed = speed;
            this.texture = texture;
            moveTimer = TimeSpan.Zero;
            pathIndex = 0;
            onScreenPosition = TowerDefense.Grid[pathIndex] + new Vector2(TowerDefense.Grid.SquareSize / 2);
            debugMousePosition = Vector2.Zero;
            newAngle = true;
            debugCenter = Vector2.Zero;
        }

        public void Update(GameTime gameTime)
        {
            Vector2 targetOnScreenPosition = TowerDefense.Grid[pathIndex + 1] + new Vector2(TowerDefense.Grid.SquareSize / 2);//TowerDefense.Grid.Map.Path[pathIndex+1]; // TODO: Get next target
            //var thing = TowerDefense.Grid[targetOnScreenPosition];
            Vector2 gridSquarePosotionToBaseMovementOn = TowerDefense.Grid[pathIndex];// + new Vector2(TowerDefense.Grid.SquareSize / 2);
            GridSquare gridSquareToBaseMovementOn = TowerDefense.Grid[onScreenPosition];//onScreenPosition
            if (gridSquareToBaseMovementOn == null && pathIndex < 1)
            {
                gridSquareToBaseMovementOn = TowerDefense.Grid[targetOnScreenPosition];
            }
            else
            {
                //TowerDefense.U
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
                        debugCenter = targetOnScreenPosition;
                        if (Math.Abs(test.X) < 1f && Math.Abs(test.Y) < 1f && pathIndex + 2 < TowerDefense.Grid.Map.Path.Count)
                        {
                            pathIndex++;
                        }
                    }
                    break;

                case Grid.TileKinds.TurnFromEntryLeftToUpperRight:
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

                            onScreenPosition.Y = (float)Math.Floor(onScreenPosition.Y)-1;
                            //onScreenPosition.X = (float)Math.Ceiling(onScreenPosition.X);
                            newAngle = true;
                        }
                    }
                    break;
                case Grid.TileKinds.TurnFromEntryBelowToUpperLeft:
                    break;
                case Grid.TileKinds.TurnFromEntryRightToLowerLeft:
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
                        Vector2 centerOfRotation = new Vector2(gridPosition.X+1, gridPosition.Y+1) * TowerDefense.Grid.SquareSize;

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
                        }
                    }

                    break;
                case Grid.TileKinds.TurnFromEntryAboveToLowerRight:
                    {
                        if (newAngle)
                        {
                            currentAngle = 180;
                            newAngle = false;
                        }
                        float distanceToTravel = .5f * (float)Math.PI * TowerDefense.Grid.SquareSize / 2;
                        float thyme = distanceToTravel / movementSpeed;
                        float angularVelocity = 90 / thyme;

                        float targetAngle = 90;
                        Vector2 centerOfRotation = new Vector2(gridPosition.X + 1, gridPosition.Y) * TowerDefense.Grid.SquareSize;

                        float radius = TowerDefense.Grid.SquareSize / 2;

                        debugCenter = centerOfRotation;

                        //Debug.WriteLine(centerOfRotation - debugMousePosition);

                        targetOnScreenPosition = RotateWalk(radius, currentAngle - 1, centerOfRotation);
                        //var moveAmount = (float)(movementSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                        //var moveDirection = targetOnScreenPosition - onScreenPosition;
                        //moveDirection.Normalize();

                        //Debug.WriteLine(currentAngle);

                        //var moveVector = moveDirection * moveAmount;
                        //onScreenPosition += moveVector;

                        onScreenPosition = targetOnScreenPosition;

                        //Vector2 test = onScreenPosition - targetOnScreenPosition;
                        //if (Math.Abs(test.X) < 1f && Math.Abs(test.Y) < 1f) //&& pathIndex + 2 < TowerDefense.Grid.Map.Path.Count)
                        //{
                            currentAngle -= angularVelocity* (float)(gameTime.ElapsedGameTime.TotalSeconds);
                        //}
                        if (currentAngle <= targetAngle)
                        {
                            pathIndex++;

                            // Round up, because floats
                            onScreenPosition.Y = (float)Math.Ceiling(onScreenPosition.Y);
                            onScreenPosition.X = (float)Math.Ceiling(onScreenPosition.X);

                            newAngle = true;
                        }
                    }
                    break;

                case Grid.TileKinds.None:
                default:
                    break;
            }

            var mouseState = Mouse.GetState();
            debugMousePosition = mouseState.Position.ToVector2();



            //Debug.WriteLine(TowerDefense.Grid[targetOnScreenPosition].TileKind);


            //var moveAmount = (float)(movementSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            //var moveDirection = targetOnScreenPosition - onScreenPosition;
            //moveDirection.Normalize();

            //var moveVector = moveDirection * moveAmount;
            //onScreenPosition += moveVector;
            //Vector2 test = onScreenPosition - targetOnScreenPosition;

            //if (Math.Abs(test.X) < .01f && Math.Abs(test.Y) < .01f && pathIndex+2 < TowerDefense.Grid.Map.Path.Count)
            //{
            //    pathIndex++;
            //}
            //Debug.WriteLine(onScreenPosition);
        }

        Vector2 RotateWalk(float radius, float angle, Vector2 center)
        {
            return new Vector2((radius * (float)Math.Cos(angle * Math.PI / 180)) + center.X, (radius * (float)Math.Sin(angle * Math.PI / 180)) + center.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, new Rectangle((onScreenPosition).ToPoint()/*(Grid.Map.Path[debugMapTraverserPositionIndex] * Grid.SquareSize).ToPoint()*/, new Point(TowerDefense.Grid.SquareSize / 4, TowerDefense.Grid.SquareSize / 4)), Color.Red);

            var drawPosition = onScreenPosition;// + new Vector2(TowerDefense.Grid.SquareSize / 2);
            spriteBatch.Draw(texture, debugCenter, null, Color.Red, rotation: 0f, origin: Vector2.One / 2f, scale: Vector2.One * 20, SpriteEffects.None, layerDepth: 0f);

            spriteBatch.Draw(texture, drawPosition, null, Color.Blue, rotation: 0f, origin: Vector2.One / 2f, scale: Vector2.One * 20, SpriteEffects.None, layerDepth: 0f);
        }

    }
}
