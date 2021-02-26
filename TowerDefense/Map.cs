using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace TowerDefense
{
    class Map
    {
        Vector2 mapSize;
        List<Vector2> path;

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
        void ImportMap(string mapFilePath)
        {
            //string[] mapLines = File.ReadAllLines("mapFilePath");


            string[] mapLines = File.ReadAllLines(mapFilePath);
            string regexPattern = @"^(?<x>\d+)[:, ](?<y>\d+)$";
            var regex = new Regex(regexPattern);
            List<Vector2> data = new List<Vector2>();
            foreach (var line in mapLines)
            {
                foreach (Match m in regex.Matches(line))
                {
                    data.Add(new Vector2(int.Parse(m.Groups["x"].Value), int.Parse(m.Groups["y"].Value)));
                }
            }

            mapSize = data[0];
            for (int i = 1; i < data.Count-1; i++)
            {
                var line = pathConnectionHelper(data[i], data[i+1]);
                for (int k = 0; k < line.Count; k++)
                {
                    path.Add(line[i]);
                }
            }
            path.Add(data[data.Count - 1]);
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
            if (start.Y - end.Y > 0)
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
            while(currentPosition.X != endPoint.X)
            {
                linePath.Add(currentPosition);
                currentPosition.X += lineDirection.X;
            }
            return linePath;
        }
    }
}
