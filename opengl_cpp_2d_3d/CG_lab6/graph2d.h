#pragma once

#include "graph.h"
#include <vector>
#include <GL/glut.h>

class Graph2d : public Graph
{
private:
	std::vector<Point> points;
	Point curPoint;
	bool closed = false;
	bool viewPoints = false;
	
public:
	Graph2d(int *argc, char **argv, std::string title);

	Graph2d(int *argc, char **argv, std::string title, Point *size);

	void render(void);
	
	void onMouseMove(int x, int y);
	
	void onMouseClick(int button, int state, int x, int y);
	
	void onKeyPress(unsigned char key, int x, int y);
};
