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

namespace TowerDefense
{
    class Map
    {
        public BasicVector2 mapSize { get; set; }
        public List<BasicVector2> vertices { get; set; }
        public List<Vector2> path { get; set; }


        public Map()
        {
            mapSize = Vector2.Zero;
            path = new List<Vector2>();

        }

        public void MapFileFormatTest(string filePath)
        {
            Map testMap = new Map();
            testMap.mapSize = new Vector2(20, 20);
            testMap.vertices = new List<BasicVector2>()
            {
                { new BasicVector2(0, 2)},
                { new BasicVector2(2, 5)},
                { new BasicVector2(5, 7)}
            };
            var jsonText = JsonSerializer.Serialize(testMap); // Formatting.Indented;
            File.WriteAllText(filePath, jsonText);
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
        public void ImportMap(string mapFilePath)
        {
            //string[] mapLines = File.ReadAllLines("mapFilePath");

            //var test1 = JsonConvert.SerializeObject(Map);
            string fileContents = File.ReadAllText(mapFilePath);
            var map = JsonSerializer.Deserialize<Map>(fileContents);

            mapSize = map.mapSize;
            vertices = map.vertices;

            //string[] mapLines = File.ReadAllLines(mapFilePath);
            //string regexPattern = @"^(?<x>\d+)[:, ]{1,2}(?<y>\d+)$";
            //var regex = new Regex(regexPattern);
            //List<Vector2> data = new List<Vector2>();
            //foreach (var line in mapLines)
            //{
            //    foreach (Match m in regex.Matches(line))
            //    {
            //        data.Add(new Vector2(int.Parse(m.Groups["x"].Value), int.Parse(m.Groups["y"].Value)));
            //    }
            //}

            //mapSize = data[0];
            //for (int i = 1; i < data.Count-1; i++)
            //{
            //    var line = pathConnectionHelper(data[i], data[i+1]);
            //    for (int k = 0; k < line.Count; k++)
            //    {
            //        path.Add(line[k]);
            //    }
            //}
            //path.Add(data[data.Count - 1]);

            //var test2 = JsonConvert.DeserializeObject<List<Vector2>>(test1);

            for (int i = 0; i < vertices.Count - 1; i++)
            {

                var line = pathConnectionHelper(vertices[i].ToVector2(), vertices[i + 1].ToVector2());
                for (int k = 0; k < line.Count; k++)
                {
                    path.Add(line[k]);
                }
            }
            path.Add(vertices[vertices.Count - 1].ToVector2());
            
        }

        (int X, int Y) pathDirectionHelper(Vector2 start, Vector2 end)
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

        List<Vector2> pathConnectionHelper(Vector2 startPoint, Vector2 endPoint)
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
