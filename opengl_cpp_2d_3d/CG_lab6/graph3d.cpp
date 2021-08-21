#include "graph3d.h"
#include <iostream>

Graph3d::Graph3d(int *argc, char **argv, std::string title) : Graph(argc, argv, title)
{
	glMatrixMode(GL_MODELVIEW);
	glClearColor(0.0, 0.1, 0.5, 0.0);

	// рассчет освещения
	glEnable(GL_LIGHTING);

	// двухсторонний расчет освещения
	glLightModelf(GL_LIGHT_MODEL_TWO_SIDE, GL_TRUE);

	// автоматическое приведение нормалей к
	// единичной длине
	glEnable(GL_NORMALIZE);
}

Graph3d::Graph3d(int *argc, char **argv, std::string title, Point *size) : Graph(argc, argv, title, size)
{
	glMatrixMode(GL_MODELVIEW);
	glClearColor(0.0, 0.1, 0.5, 0.0);

	// рассчет освещения
	glEnable(GL_LIGHTING);

	// двухсторонний расчет освещения
	glLightModelf(GL_LIGHT_MODEL_TWO_SIDE, GL_TRUE);

	// автоматическое приведение нормалей к
	// единичной длине
	glEnable(GL_NORMALIZE);
}

void Graph3d::render(void)
{
	glClear(GL_COLOR_BUFFER_BIT);

	glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);

	// свойства материала
	GLfloat material_diffuse[] = { 0.3, 0.3, 0.3, 0.6 };
	glMaterialfv(GL_FRONT_AND_BACK, GL_AMBIENT, material_diffuse);

	// направленный источник света
	GLfloat light_ambient[] = { 0.5, 0.3, 0.8 };
	GLfloat light_direction[] = { 2.0, 2.0, 2.0, 5.0 };
	glEnable(GL_LIGHT0);
	glLightfv(GL_LIGHT0, GL_AMBIENT, light_ambient);
	glLightfv(GL_LIGHT0, GL_POSITION, light_direction);

	glColor3ub(255, 255, 255);
	if (tmode)
		glutSolidTorus(0.1, 0.5, 2, 250);
	else
		glutSolidOctahedron();

	// отключение источников света
	glDisable(GL_LIGHT0);

	glutSwapBuffers();
	glFlush();
}

void Graph3d::onKeyPress(unsigned char key, int x, int y)
{
	switch (key) {
	case 'a':
		glRotated(-1.5, 0, 1, 0);
		break;
	case 'd':
		glRotated(1.5, 0, 1, 0);
		break;
	case 'w':
		glRotated(-1.5, 1, 0, 0);
		break;
	case 's':
		glRotated(1.5, 1, 0, 0);
		break;
	case '-':
		glScaled(0.7, 0.7, 0.7);
		break;
	case '+':
		glScaled(1.3, 1.3, 1.3);
		break;
	case 'm':
		tmode = !tmode;
		break;
	}


	glutPostRedisplay();
}