#include "graph2d.h"
#include "graph3d.h"

#include <GL/glut.h>

#include <vector>
#include <iostream>
#include <thread>

#include "point.h"

using namespace std;

Graph *graph = NULL;

void onMouseClick(int button, int state, int x, int y)
{
	graph->onMouseClick(button, state, x, y);
}

void onMouseMove(int x, int y)
{
	graph->onMouseMove(x, y);
}

void onKeyPress(unsigned char key, int x, int y)
{
	graph->onKeyPress(key, x, y);
}

void idle(void)
{
	graph->idle();
}

void render(void)
{
	graph->render();
}

int main(int argc, char **argv)
{
	bool valid = false;
	while (!valid)
	{
		cout << "Input mode (2d/3d) -> ";
		string cmd = "";
		cin >> cmd;

		if (cmd == "2d")
		{
			graph = new Graph2d(&argc, argv, "CG lab ¹6 | 2D mode", new Point(640, 480));
			valid = true;
		}
		else if (cmd == "3d")
		{
			graph = new Graph3d(&argc, argv, "CG lab ¹6 | 3D mode", new Point(640, 640));
			valid = true;
		}
		else cout << "Invalid command!" << endl;
	}

	//Register execute functions
	glutDisplayFunc(render);
	glutPassiveMotionFunc(onMouseMove);
	glutMouseFunc(onMouseClick);
	glutKeyboardFunc(onKeyPress);

	//Run main loop
	glutMainLoop();
	delete graph;

	return 0;
}
