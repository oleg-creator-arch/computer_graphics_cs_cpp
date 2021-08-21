#include "graph2d.h"

Graph2d::Graph2d(int *argc, char **argv, std::string title) : Graph(argc, argv, title)
{
	glClearColor(0.0, 0.1, 0.5, 0.0);
	glMatrixMode(GL_PROJECTION);
	glOrtho(0.0f, (float)windowSize->X, 0.0f, (float)windowSize->Y, -1.0, 1.0);
};

Graph2d::Graph2d(int *argc, char **argv, std::string title, Point *size) : Graph(argc, argv, title, size)
{
	glClearColor(0.0, 0.1, 0.5, 0.0);
	glMatrixMode(GL_PROJECTION);
	glOrtho(0.0f, (float)windowSize->X, 0.0f, (float)windowSize->Y, -1.0, 1.0);
};

void Graph2d::render(void)
{
	glClear(GL_COLOR_BUFFER_BIT);
	
	glPolygonMode(GL_FRONT, GL_FILL);
	if (!points.empty())
	{
		if (viewPoints)
			glBegin(GL_POINTS);
		else
			glBegin((closed ? GL_POLYGON : GL_LINE_STRIP));
		
			glColor3ub(255, 255, 255);
			for(Point point : points)
			{
				glVertex2f((float)point.X, (float)point.Y);
			}
			if (!closed) glVertex2f((float)curPoint.X, (float)curPoint.Y);
		glEnd();
	}
	glutSwapBuffers();
	glFlush();
}

void Graph2d::onMouseMove(int x, int y)
{
	curPoint.X = x;
	curPoint.Y = windowSize->Y - y;
	
	glutPostRedisplay();
}
	
void Graph2d::onMouseClick(int button, int state, int x, int y)
{
	curPoint.X = x;
	curPoint.Y = windowSize->Y - y;
	
	if (state == GLUT_DOWN)
	{
		if (button == GLUT_LEFT_BUTTON)
		{
			if (closed) points.clear();
			closed = false;
			points.push_back(curPoint);
		}
		if (button == GLUT_RIGHT_BUTTON)
		{
			closed = true;
		}
	}
}
	
void Graph2d::onKeyPress(unsigned char key, int x, int y)
{
	if (key == 27) {
		exit(0);
	}
	if (key == 'm'){
		viewPoints = !viewPoints;
		glutPostRedisplay();
	}
}
