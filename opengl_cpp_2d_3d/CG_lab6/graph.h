#pragma once

#include <string>
#include <gl/glut.h>
#include "point.h"

class Graph
{
protected:
	Point *windowSize;
	std::string title;
public:
	Graph(int *argc, char **argv, std::string title);
	
	Graph(int *argc, char **argv, std::string title, Point *size);
	
	virtual void idle(void){};
	
	virtual void render(void) = 0;
	
	virtual void onMouseMove(int x, int y){};
	
	virtual void onMouseClick(int button, int state, int x, int y){};
	
	virtual void onKeyPress(unsigned char key, int x, int y){};
}; 
