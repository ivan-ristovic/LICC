﻿static void f(int a, const int b)
{
	const int x = 5, y;
	unsigned short z = x + y;
	signed int k = 3 + 4 * (5 - 6);
	char* s = "abcd";
	double w = 3.4 * 7.11 / 2.33;
label:
	x = 4;
	goto label;
	return a + b + x + y;
}

extern int foo(
	const float bar
)
{
	const time_t elapsed_time = time(NULL);

	bool b = 3 > 4 && 3 < 5 || 4 >= 2;
	if (5 > 3) {
		b = 1 != 1;
	} else {
		float y;
		y = 3.2;
	}

	if (3) {
		bool c = 1 != 2;
		c = b;
		f(3, 4);
	}

	if (1)
		b = 1 >= 1;

	;

	return 5;
}