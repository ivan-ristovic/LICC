﻿using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.Extensions;
using RICC.Tests.AST.Builders.C;

namespace RICC.Tests.AST
{
    internal sealed class JsonSerializationTests
    {
        [Test]
        public void SerializationTests()
        {
            this.AssertSerialization("void f() {}");
            this.AssertSerialization("int main() { return 0; }");
            this.AssertSerialization("int main(int x) { return 0; }");
            this.AssertSerialization("int main(int x, ...) { return 0; }");
            this.AssertSerialization("static extern int main(const int x, ...) { return 0; }");
            this.AssertSerialization("static const volatile bool x;");
            this.AssertSerialization("extern bool x();");
            this.AssertSerialization("bool f(); const time_t t1, t2, t3 = 2");
            this.AssertSerialization("void f() { int x; if (1) { x = 1; } else { x = 2; } }");
            this.AssertSerialization("void f() { int x; if (1) x = 1; else x = 2; }");
            this.AssertSerialization("void f() { int x; if (1) x = 1; }");
            this.AssertSerialization(@"
                extern static time_t foo_extern(int x, const int y);

                static void f(int a, const int b)
                {
                    const int x = 5, y;
                    unsigned short z = x + y;
                    signed int k = 3 + 4 * (5 - 6);
                    double w = 3.4 * 7.11 / 2.33;
                label:
                    x = 4;
                    goto label;
                    return a + b + x + y;
                }

                extern int foo(const float bar, int x, float y[], signed int z, unsigned long long k, ...) 
                {
                    const time_t elapsed_time = time(NULL);
                    int empty[10];
                    int a, w[3] = { 1, (2 * 3 + 4), (3 << 2) };

                    w[1] = 4;

                    bool b = 3 > 4 && 3 < 5 || 4 >= 2;
                    if (5 > 3) {
                        b = 1 != 1;
                    } else {
                        float y = 3.01;
                        y *= 3.2;
                    }

                    if (3) {
                        bool c = 1 != 2;
                        c = b;
                        f(3, 4);
                    }

                    if (1)
                        y = (w[1] > 5) ? 10.3 : 5.44;

                    ;

                    return a++;
                }

                static const int x = 7;
                static float y, z = 3;

                int main() 
                {
                    int x = 1;
                    while (1) {
                        if (x < 10)
                            printf(""lt 10"");
                        if (x == 17)
                            break;
                        x++;
                    }

                    for (int i = 0; i< 100; i++) {
                        foo(1, 2, 3, 4);
                        if (i > 10)
                            return 1;
                    }

                    for (x--; x< 10; x *= 2, i++) {
                        printf(""%d\n"", x);
                    }

                    return 0;
                }"
            );
        }


        private void AssertSerialization(string src)
        {
            ASTNode ast = CASTProvider.BuildFromSource(src);
            string? normal = null;
            string? compact = null;
            Assert.That(() => { normal = ast.ToJson(compact: false); }, Throws.Nothing);
            Assert.That(() => { compact = ast.ToJson(compact: true); }, Throws.Nothing);
            Assert.That(normal, Is.Not.Null);
            Assert.That(compact, Is.Not.Null);
            Assert.That(normal, Has.Length.GreaterThan(compact!.Length));
        }
    }
}