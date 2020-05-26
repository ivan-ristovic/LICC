int x = vx, y = vy;

void swap() 
{
    x = x + y;
	y = x - y;
	x = x - y;
}
