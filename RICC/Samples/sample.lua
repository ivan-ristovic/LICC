test = 1

a, b = 3, 4, 5
x, y, z, a = 3, 4, 5, 6, 7
n = nil
str = "abc"
t, f = true, false
diff, type, test = 9, "a", true

local x1, x2
local x3, x4, x5 = 6, 'a'

arr['a'] = 100
arr['b'] = 200
arr['c'] = 150

--arr = { 1, 2, 3, '4'}

--dict = {}
--dict.x = 5

function fact (n)
  if n == 0 then
    return 1
  else
    return n * fact(n-1)
  end
end
    
