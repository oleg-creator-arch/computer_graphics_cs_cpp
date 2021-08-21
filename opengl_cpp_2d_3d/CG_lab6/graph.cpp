#include "graph.h"

Graph::Graph(int *argc, char **argv, std::string title)
{
	this->title = title;
	this->windowSize = new Point(640, 480);

	//Initialization
	glutInit(argc, argv);
	glutInitWindowSize(windowSize->X, windowSize->Y);
	glutInitWindowPosition(0, 0);

	//Show Window
	glutCreateWindow(title.c_str());

	//Set mode
	glutInitDisplayMode(GLUT_RGBA | GLUT_DOUBLE | GLUT_DEPTH);
}

Graph::Graph(int *argc, char **argv, std::string title, Point *size)
{
	this->title = title;
	this->windowSize = size;

	//Initialization
	glutInit(argc, argv);
	glutInitWindowSize(windowSize->X, windowSize->Y);
	glutInitWindowPosition(0, 0);

	//Show Window
	glutCreateWindow(title.c_str());

	//Set mode
	glutInitDisplayMode(GLUT_RGBA | GLUT_DOUBLE | GLUT_DEPTH);
}
