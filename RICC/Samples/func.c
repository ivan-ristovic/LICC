static void f(int a, int b)
{
	int x = 5, y;
	unsigned short z = x + y;
	signed int k = 3 + 4 * (5 - 6);
	char* s = "abcd";
	double w = 3.4 * 7.11 / 2.33;
	return a + b + x + y;
}

extern int foo(
	float bar
)
{
	bool b = 3 > 4 && 3 < 5 || 4 > 2;
	if (5 > 3) {
		int x;
	} else {
		float y;
	}

	if (3) {
		bool c = 1 != 2;
	}

	;

	return 5;
}