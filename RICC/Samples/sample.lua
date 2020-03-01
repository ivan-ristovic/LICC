test = 1

a, b = 3, 4, 5
x, y, z, a = 3, 4, 5, 6, 7
n = nil
str = "abc"
t, f = true, false
diff, type, test = 9, "a", true

function fact (n)
  if n == 0 then
    return 1
  else
    return n * fact(n-1)
  end
end
    
