using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.Json;
using System.Drawing;
using System.Text.Json.Serialization;

namespace TowerDefense
{
    public class Map
    {
        public Vector2 MapSize { get; set; }
        public List<Vector2> Vertices { get; set; }
        public Vector2 WalkInFrom { get; set; }
        public Vector2 WalkOutFrom { get; set; }
        //public int SquareSize { get; set; }
        public Dictionary<Grid.TileKinds, String> TileTextures { get; set; }
        public List<Vector2> Path { get; set; }
        


        public Map() { }



        public void MapFileFormatTest(string filePath)
        {
            Map testMap = new Map
            {
                MapSize = new Vector2(20, 20),
                Vertices = new List<Vector2>()
                {
                    { new Vector2(0, 2) },
                    { new Vector2(2, 5) },
                    { new Vector2(5, 7) }
                },
                TileTextures = new Dictionary<Grid.TileKinds, string>()
                {
                    [Grid.TileKinds.None] = "Tiles\\land",
                    [Grid.TileKinds.Horizontal] = "Tiles\\horizontalTest340",
                    [Grid.TileKinds.Vertical] = "Tiles\\vertical340"
                }
            };
            Vector2 test = new Vector2(10, 10);

            //JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();

            //var jsonConverter = jsonSerializerOptions.GetConverter(typeof(Vector2));

            var options = new JsonSerializerOptions { IncludeFields = true, WriteIndented = true };
            var jsonText = JsonSerializer.Serialize(testMap, options); // Formatting.Indented;
            File.WriteAllText(filePath, jsonText);
            var fileContents = File.ReadAllText(filePath);
            var readingBackTest = JsonSerializer.Deserialize<Map>(fileContents, options);
        }




        /// <summary>
        /// Map file format:
        /// width
        /// height
        /// or use regex: (?<width>\d+)[:, ](?<height>\d+)
        /// or this : (?<x>\d+)[:, ](?<y>\d+)
        /// 
        /// use same regex for the rest of the stuff:
        ///start point
        ///as many vertexies as there will be, the last one will be the end point
        /// </summary>
        public static Map ImportMap(string mapFilePath)
        {
            string fileContents = File.ReadAllText(mapFilePath);
            var options = new JsonSerializerOptions { IncludeFields = true, WriteIndented = true };
            var map = JsonSerializer.Deserialize<Map>(fileContents, options);

            map.Path.Add(map.WalkInFrom);
            for (int i = 0; i < map.Vertices.Count - 1; i++)
            {

                var line = pathConnectionHelper(map.Vertices[i], map.Vertices[i + 1]);
                for (int k = 0; k < line.Count; k++)
                {
                    map.Path.Add(line[k]);
                }
            }
            map.Path.Add(map.Vertices[map.Vertices.Count - 1]);
            map.Path.Add(map.WalkOutFrom);

            return map;


            static (int X, int Y) pathDirectionHelper(Vector2 start, Vector2 end)
            {
                //mod itself+1
                int x = 0;
                //x %= x + 1;//this doent work if going right to left
                //x -= x-1;
                int y = 0;

                if (start.X - end.X < 0)
                {
                    x = 1;
                }
                else if (start.X - end.X > 0)
                {
                    x = -1;
                }
                if (start.Y - end.Y < 0)
                {
                    y = 1;
                }
                else if (start.Y - end.Y > 0)
                {
                    y = -1;
                }

                return (x, y);
            }

            static List<Vector2> pathConnectionHelper(Vector2 startPoint, Vector2 endPoint)
            {
                List<Vector2> linePath = new List<Vector2>();
                var lineDirection = pathDirectionHelper(startPoint, endPoint);
                var currentPosition = startPoint;
                while (currentPosition.Y != endPoint.Y)
                {
                    linePath.Add(currentPosition);
                    currentPosition.Y += lineDirection.Y;
                }
                while (currentPosition.X != endPoint.X)
                {
                    linePath.Add(currentPosition);
                    currentPosition.X += lineDirection.X;
                }
                return linePath;
            }
        }
    }
}
