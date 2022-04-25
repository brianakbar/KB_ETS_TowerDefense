from collections import defaultdict
import Queue
import sys

class Graph:

	# Constructor
    def __init__(self):
        self.graph = defaultdict(dict)
        self.heuristic = defaultdict(int)
        self.previous_node = defaultdict(str)

    def clear_graph(self):
        self.graph.clear()
        self.heuristic.clear()
        self.previous_node.clear()

	# function to add an edge to graph
    def add_edge(self, u, v, j):
        self.graph[u][v] = j
        self.graph[v][u] = j
        
    # function to add heuristic number
    def add_heuristic(self, u, v):
        self.heuristic[u] = v
    
    def find(self, s):
        self.previous_node.clear()
        g = defaultdict(int)
        
        for node in self.graph:
            g[node] = sys.maxsize
        g[s] = 0
        pq = Queue.PriorityQueue()
        
        self.previous_node[s] = 0
        
        while self.heuristic[s] != 0:
            for node in self.graph[s]:
                temp_g = g[s] + self.graph[s][node]
                if temp_g < g[node]:
                    pq.put((temp_g + self.heuristic[node], node))
                    g[node] = temp_g
                    self.previous_node[node] = s
            s = pq.get()[1]
        return self.get_path(s)
    
    def get_path(self, s):
        #Get Shortest Path and Cost
        path = list()
        cost = int()
        while s != 0:
            path.append(s)
            if self.previous_node[s] != 0:
                cost += self.graph[s][self.previous_node[s]]
            s = self.previous_node[s]
            
        path.reverse() 
        return path


