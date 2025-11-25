using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
      	int INF = 99999999;

        string firstLine = Console.ReadLine();
        int vertices = int.Parse(firstLine);
        int[,] dist = new int[vertices, vertices];
        
        for(int i = 0; i < vertices; i++) 
        {
            string[] line = Console.ReadLine().Split(' ');
            for(int j = 0; j < vertices; j++) 
            {
                if(line[j] == "INF") 
                {
                    dist[i, j] = INF;
                } 
                else 
                {
                    dist[i, j] = int.Parse(line[j]);
                }
            }
        }
        
        if (FloydWarshall(dist, vertices))
        {
            PrintSolution(dist, vertices);
        }
        else
        {
            Console.WriteLine("NEGATIVE CYCLE");
        }
    }
    
    public static bool FloydWarshall (int[,] graph, int vertices)
    {
        int[,] dist = new int[vertices, vertices];
        int INF = 99999999;
      
        for(int i = 0; i < vertices; i++) 
        {
            for(int j = 0; j < vertices; j++) 
            {
                dist[i, j] = graph[i, j];
            }
        }
        
        for(int k = 0; k < vertices; k++) 
        {
            for(int i = 0; i < vertices; i++) 
            {
                for(int j = 0; j < vertices; j++) 
                {
                    if(dist[i, k] != INF && dist[k, j] != INF && dist[i, k] + dist[k, j] < dist[i, j]) 
                    {
                        dist[i, j] = dist[i, k] + dist[k, j];
                    }
                }
            }
        }
        
        for(int i = 0; i < vertices; i++) 
        {
            if(dist[i, i] < 0) 
            {
                return false; 
            }
        }

        for(int i = 0; i < vertices; i++) 
        {
            for(int j = 0; j < vertices; j++) 
            {
                graph[i, j] = dist[i, j];
            }
        }

        return true;
    }
    
    public static void PrintSolution(int[,] dist, int vertices)
    {
      	int INF = 99999999;
        for(int i = 0; i < vertices; i++) 
        {
            string[] columna = new string[vertices];
            for(int j = 0; j < vertices; j++) 
            {
                if(dist[i, j] >= INF) 
                {
                    columna[j] = "INF";
                } 
                else 
                {
                    columna[j] = dist[i, j].ToString();
                }
            }
            Console.WriteLine(string.Join(" ", columna));
        }
    }
}                                  