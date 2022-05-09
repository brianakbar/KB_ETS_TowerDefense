from collections import defaultdict
import Queue
import sys

class Graph:

	# Constructor
    def __init__(self):
        self.graph = defaultdict(dict)
        self.heuristic = defaultdict(int)
        self.previous_node = defaultdict(str)
        self.destination_node = object
        self.start_node = object

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
    
    def find(self, start_node):
        self.previous_node.clear()
        cost_to = defaultdict(int)
        
        explored = list()
        
        for node in self.graph:
            cost_to[node] = sys.maxsize
        cost_to[start_node] = 0
        pq = Queue.PriorityQueue()
        
        self.previous_node[start_node] = 0
        
        #current_node = start_node
        self.start_node = start_node
        pq.put((self.heuristic[start_node], start_node))
        while pq.empty() == False:
            current_node = pq.get()[1]
            explored.append(current_node)
            if self.heuristic[current_node] == 0:
                break
            for neighbor_node in self.graph[current_node]:
                temp_cost = cost_to[current_node] + self.graph[current_node][neighbor_node]
                if temp_cost < cost_to[neighbor_node]:
                    explored.append(current_node)
                    pq.put((temp_cost + self.heuristic[neighbor_node], neighbor_node))
                    cost_to[neighbor_node] = temp_cost
                    self.previous_node[neighbor_node] = current_node  
        explored.append(current_node)
        self.destination_node = current_node
        return explored
    
    def get_path(self):
        #Get Shortest Path
        path = list()

        if self.heuristic[self.destination_node] != 0:
            path.append(self.start_node)
        else:
            current_node = self.destination_node
            while current_node != 0:
                path.append(current_node)
                current_node = self.previous_node[current_node]   
            path.reverse() 

        return path


