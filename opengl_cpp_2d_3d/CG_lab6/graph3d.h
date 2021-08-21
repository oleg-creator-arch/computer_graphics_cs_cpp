#pragma once

#include "graph.h"

class Graph3d : public Graph
{
private:
	bool tmode = false;
public:
	Graph3d(int *argc, char **argv, std::string title);

	Graph3d(int *argc, char **argv, std::string title, Point *size);

	void render(void);

	void onKeyPress(unsigned char key, int x, int y);
};

