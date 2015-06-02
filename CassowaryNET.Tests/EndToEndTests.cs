using System;
using System.Linq;
using CassowaryNET.Constraints;
using CassowaryNET.Exceptions;
using CassowaryNET.Variables;
using NUnit.Framework;

namespace CassowaryNET.Tests
{
    [TestFixture]
    public class EndToEndTests
    {
        private static readonly Random random = new Random();

        private static CassowarySolver GetTarget()
        {
            return new CassowarySolver();
        }

        [Test]
        public void can_solve_simple_constraint()
        {
            var x = new ClVariable(167d);
            var y = new ClVariable(2d);
            var target = GetTarget();

            target.AddConstraint(x == y);

            Assert.That(x.Value, Is.EqualTo(y.Value));
        }

        [Test]
        public void can_solve_stays()
        {
            var x = new ClVariable(5d);
            var y = new ClVariable(10d);
            var target = GetTarget();

            target.AddStay(x);
            target.AddStay(y);

            Assert.That(x.Value, IsX.Approx(5d));
            Assert.That(y.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_variable_greater_than_constant()
        {
            var x = new ClVariable(5d);
            var target = GetTarget();

            target.AddConstraint(x >= 10d);

            Assert.That(x.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_variable_less_than_constant()
        {
            var x = new ClVariable(10d);
            var target = GetTarget();

            target.AddConstraint(x <= 5d);

            Assert.That(x.Value, IsX.Approx(5d));
        }

        [Test]
        public void can_solve_variable_equal_to_constant()
        {
            var x = new ClVariable(5d);
            var target = GetTarget();

            target.AddConstraint(x == 10d);

            Assert.That(x.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_constant_greater_than_variable()
        {
            var x = new ClVariable(10d);
            var target = GetTarget();

            target.AddConstraint(5d <= x);

            Assert.That(x.Value, IsX.Approx(5d));
        }

        [Test]
        public void can_solve_constant_less_than_variable()
        {
            var x = new ClVariable(5d);
            var target = GetTarget();
            
            target.AddConstraint(10 <= x);

            Assert.That(x.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_greater_than_with_stay()
        {
            var x = new ClVariable(10d);
            var width = new ClVariable(10d);
            var rightMin = new ClVariable(100d);

            var right = x + width;

            var target = GetTarget();
            
            target.AddStay(width);
            target.AddStay(rightMin);
            target.AddConstraint(right >= rightMin);

            Assert.That(x.Value, IsX.Approx(90d));
            Assert.That(width.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_less_than_with_stay()
        {
            var x = new ClVariable(200d);
            var width = new ClVariable(10d);
            var rightMin = new ClVariable(100d);

            var right = x + width;

            var target = GetTarget();

            target.AddStay(width);
            target.AddStay(rightMin);
            target.AddConstraint(right <= rightMin);

            Assert.That(x.Value, IsX.Approx(90d));
            Assert.That(width.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_equal_to_with_stay()
        {
            var x = new ClVariable(10d);
            var width = new ClVariable(10d);
            var rightMin = new ClVariable(100d);

            var right = x + width;

            var target = GetTarget();
            
            target.AddStay(width);
            target.AddStay(rightMin);
            target.AddConstraint(right == rightMin);

            Assert.That(x.Value, IsX.Approx(90d));
            Assert.That(width.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_greater_than_with_variable()
        {
            var x = new ClVariable(10d);
            var width = new ClVariable(10d);
            var rightMin = new ClVariable(100d);

            var right = x + width;

            var target = GetTarget();

            target.AddStay(width);
            target.AddStay(rightMin);
            target.AddConstraint(right >= rightMin);

            Assert.That(x.Value, IsX.Approx(90d));
            Assert.That(width.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_less_than_with_variable()
        {
            var x = new ClVariable(200d);
            var width = new ClVariable(10d);
            var rightMin = new ClVariable(100d);

            var right = x + width;

            var target = GetTarget();
            
            target.AddStay(width);
            target.AddStay(rightMin);
            target.AddConstraint(right <= rightMin);

            Assert.That(x.Value, IsX.Approx(90d));
            Assert.That(width.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_equal_to_with_expression()
        {
            var x1 = new ClVariable(10d);
            var width1 = new ClVariable(10d);
            var right1 = x1 + width1;

            var x2 = new ClVariable(100d);
            var width2 = new ClVariable(10d);
            var right2 = x2 + width2;

            var target = GetTarget();
            
            target.AddStay(width1);
            target.AddStay(width2);
            target.AddStay(x2);
            target.AddConstraint(right1 == right2);

            Assert.That(x1.Value, IsX.Approx(100d));
            Assert.That(x2.Value, IsX.Approx(100d));
            Assert.That(width1.Value, IsX.Approx(10d));
            Assert.That(width2.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_greater_than_with_expression()
        {
            var x1 = new ClVariable(10d);
            var width1 = new ClVariable(10d);
            var right1 = x1 + width1;

            var x2 = new ClVariable(100d);
            var width2 = new ClVariable(10d);
            var right2 = x2 + width2;

            var target = GetTarget();
            
            target.AddStay(width1);
            target.AddStay(width2);
            target.AddStay(x2);
            target.AddConstraint(right1 >= right2);

            Assert.That(x1.Value, IsX.Approx(100d));
        }

        [Test]
        public void can_solve_less_than_with_expression()
        {
            var x1 = new ClVariable(10d);
            var width1 = new ClVariable(10d);
            var right1 = x1 + width1;

            var x2 = new ClVariable(100d);
            var width2 = new ClVariable(10d);
            var right2 = x2 + width2;

            var target = GetTarget();

            target.AddStay(width1);
            target.AddStay(width2);
            target.AddStay(x2);
            target.AddConstraint(right2 <= right1);

            Assert.That(x1.Value, IsX.Approx(100d));
        }

        [Test]
        public void add_and_remove()
        {
            var x = new ClVariable("x");
            var target = GetTarget();

            target.AddConstraint((x == 100d).WithStrength(ClStrength.Weak));

            var c10 = (x <= 10d); 
            var c20 = (x <= 20d); 
            target.AddConstraint(c10);
            target.AddConstraint(c20);

            Assert.That(x.Value, IsX.Approx(10d));

            target.RemoveConstraint(c10);
            Assert.That(x.Value, IsX.Approx(20d));

            target.RemoveConstraint(c20);
            Assert.That(x.Value, IsX.Approx(100d));

            var c10Again = (x <= 10d); 
            target.AddConstraint(c10);
            target.AddConstraint(c10Again);
            Assert.That(x.Value, IsX.Approx(10d));

            target.RemoveConstraint(c10);
            Assert.That(x.Value, IsX.Approx(10d));

            target.RemoveConstraint(c10Again);
            Assert.That(x.Value, IsX.Approx(100d));
        }

        [Test]
        public void add_and_remove_with_2_variables()
        {
            var x = new ClVariable("x");
            var y = new ClVariable("y");
            var target = GetTarget();

            target.AddConstraint((x == 100d).WithStrength(ClStrength.Weak));
            target.AddConstraint((y == 120d).WithStrength(ClStrength.Strong));

            var c10 = x <= 10d;
            var c20 = x <= 20d;
            target.AddConstraint(c10);
            target.AddConstraint(c20);
            Assert.That(x.Value, IsX.Approx(10d));
            Assert.That(y.Value, IsX.Approx(120d));

            target.RemoveConstraint(c10);
            Assert.That(x.Value, IsX.Approx(20d));
            Assert.That(y.Value, IsX.Approx(120d));

            var cxy = (2d*x == y);
            target.AddConstraint(cxy);
            Assert.That(x.Value, IsX.Approx(20d));
            Assert.That(y.Value, IsX.Approx(40d));

            target.RemoveConstraint(c20);
            Assert.That(x.Value, IsX.Approx(60d));
            Assert.That(y.Value, IsX.Approx(120d));

            target.RemoveConstraint(cxy);
            Assert.That(x.Value, IsX.Approx(100d));
            Assert.That(y.Value, IsX.Approx(120d));
        }

        [Test]
        public void casso1()
        {
            var x = new ClVariable("x");
            var y = new ClVariable("y");
            var target = GetTarget();

            // has to break one of the constraints!
            // either solution is ok

            target.AddConstraint(x <= y);
            target.AddConstraint(y == (x + 3d));
            target.AddConstraint((x == 10d).WithStrength(ClStrength.Weak));
            target.AddConstraint((y == 10d).WithStrength(ClStrength.Weak));

            if (IsX.Approx(10d).Matches(x.Value))
            {
                Assert.That(x.Value, IsX.Approx(10d));
                Assert.That(y.Value, IsX.Approx(13d));
            }
            else
            {
                Assert.That(x.Value, IsX.Approx(7d));
                Assert.That(y.Value, IsX.Approx(10d));
            }
        }

        [Test]
        public void inconsistent1()
        {
            var x = new ClVariable("x");
            var target = GetTarget();

            target.AddConstraint(x == 10d);

            Assert.That(
                () => target.AddConstraint(x == 5d),
                Throws.TypeOf<CassowaryRequiredConstraintFailureException>());
        }

        [Test]
        public void inconsistent2()
        {
            var x = new ClVariable("x");
            var target = GetTarget();

            target.AddConstraint(x >= 10d);

            Assert.That(
                () => target.AddConstraint(x <= 5d),
                Throws.TypeOf<CassowaryRequiredConstraintFailureException>());
        }

        [Test]
        public void inconsistent3()
        {
            var w = new ClVariable("w");
            var x = new ClVariable("x");
            var y = new ClVariable("y");
            var z = new ClVariable("z");
            var target = GetTarget();

            target.AddConstraint(w >= 10d);
            target.AddConstraint(x >= w);
            target.AddConstraint(y >= x);
            target.AddConstraint(z >= y);
            target.AddConstraint(z >= 8d);

            Assert.That(
                () => target.AddConstraint(z <= 4d),
                Throws.TypeOf<CassowaryRequiredConstraintFailureException>());
        }

        [Test]
        public void inconsistent4()
        {
            var x = new ClVariable("x");
            var y = new ClVariable("y");
            var target = GetTarget();

            target.AddConstraint(x == 10d);
            target.AddConstraint(x == y);

            Assert.That(
                () => target.AddConstraint(y == 5d),
                Throws.TypeOf<CassowaryRequiredConstraintFailureException>());
        }

        [Test]
        public void multi_edit()
        {
            var x = new ClVariable("x");
            var y = new ClVariable("y");
            var w = new ClVariable("w");
            var h = new ClVariable("h");

            var target = GetTarget();

            target.AddStay(x);
            target.AddStay(y);
            target.AddStay(w);
            target.AddStay(h);

            // start an editing session
            target.AddEditVar(x);
            target.AddEditVar(y);

            // ////////
            target.BeginEdit();

            target.SuggestValue(x, 10);
            target.SuggestValue(y, 20);

            // force the system to resolve
            target.Resolve();

            Assert.That(x.Value, IsX.Approx(10d));
            Assert.That(y.Value, IsX.Approx(20d));
            Assert.That(w.Value, IsX.Approx(0d));
            Assert.That(h.Value, IsX.Approx(0d));

            // open a second set of variables for editing
            target.AddEditVar(w);
            target.AddEditVar(h);

            // // ////////
            target.BeginEdit();
            target.SuggestValue(w, 30);
            target.SuggestValue(h, 40);
            target.EndEdit();
            // // ////////

            Assert.That(x.Value, IsX.Approx(10d));
            Assert.That(y.Value, IsX.Approx(20d));
            Assert.That(w.Value, IsX.Approx(30d));
            Assert.That(h.Value, IsX.Approx(40d));

            // make sure the first set can still be edited
            target.SuggestValue(x, 50);
            target.SuggestValue(y, 60);

            target.EndEdit();
            // ////////

            Assert.That(x.Value, IsX.Approx(50d));
            Assert.That(y.Value, IsX.Approx(60d));
            Assert.That(w.Value, IsX.Approx(30d));
            Assert.That(h.Value, IsX.Approx(40d));
        }

        [Test]
        public void multi_edit2()
        {
            var x = new ClVariable("x");
            var y = new ClVariable("y");
            var w = new ClVariable("w");
            var h = new ClVariable("h");

            var target = GetTarget();

            target.AddStay(x);
            target.AddStay(y);
            target.AddStay(w);
            target.AddStay(h);

            target.AddEditVar(x);
            target.AddEditVar(y);


            // ////////
            target.BeginEdit();
            target.SuggestValue(x, 10);
            target.SuggestValue(y, 20);
            target.Resolve();
            target.EndEdit();
            // ////////

            Assert.That(x.Value, IsX.Approx(10d));
            Assert.That(y.Value, IsX.Approx(20d));
            Assert.That(w.Value, IsX.Approx(0d));
            Assert.That(h.Value, IsX.Approx(0d));


            target.AddEditVar(w);
            target.AddEditVar(h);

            // // ////////
            target.BeginEdit();
            target.SuggestValue(w, 30);
            target.SuggestValue(h, 40);
            target.EndEdit();
            // // ////////

            Assert.That(x.Value, IsX.Approx(10d));
            Assert.That(y.Value, IsX.Approx(20d));
            Assert.That(w.Value, IsX.Approx(30d));
            Assert.That(h.Value, IsX.Approx(40d));


            target.AddEditVar(x);
            target.AddEditVar(y);

            // // // ////////
            target.BeginEdit();
            target.SuggestValue(x, 50);
            target.SuggestValue(y, 60);
            target.EndEdit();
            // // // ////////

            Assert.That(x.Value, IsX.Approx(50d));
            Assert.That(y.Value, IsX.Approx(60d));
            Assert.That(w.Value, IsX.Approx(30d));
            Assert.That(h.Value, IsX.Approx(40d));
        }

        private static double RandomIn(double min, double max)
        {
            var r = random.NextDouble();
            return min + (max - min) * r;
        }

        [Test]
        public void multi_edit3()
        {
            const double Min = 100d;
            const double Max = 500d;

            var width = new ClVariable("width");
            var height = new ClVariable("height");
            var top = new ClVariable("top");
            var bottom = new ClVariable("bottom");
            var left = new ClVariable("left");
            var right = new ClVariable("right");

            var target = GetTarget();

            var iw = new ClVariable("window_innerWidth", RandomIn(Min, Max));
            var ih = new ClVariable("window_innerHeight", RandomIn(Min, Max));


            target.AddConstraint(
                (width == iw).WithStrength(ClStrength.Strong).WithWeight(0d));
            target.AddConstraint(
                (height == ih).WithStrength(ClStrength.Strong).WithWeight(0d));

            target.AddConstraint(
                (top == 0d).WithStrength(ClStrength.Weak).WithWeight(0d));
            target.AddConstraint(
                (left == 0d).WithStrength(ClStrength.Weak).WithWeight(0d));

            // right is at least left + width, &c
            target.AddConstraint(
                (bottom == top + height).WithStrength(ClStrength.Medium).WithWeight(0d));
            target.AddConstraint(
                (right == left + width).WithStrength(ClStrength.Medium).WithWeight(0d));
            
            target.AddStay(iw);
            target.AddStay(ih);

            for (int i = 0; i < 30; i++)
            {
                var iwv = RandomIn(Min, Max);
                var ihv = RandomIn(Min, Max);

                target.AddEditVar(iw);
                target.AddEditVar(ih);

                target.BeginEdit();
                target.SuggestValue(iw, iwv);
                target.SuggestValue(ih, ihv);
                target.EndEdit();

                Assert.That(top.Value, IsX.Approx(0d));
                Assert.That(left.Value, IsX.Approx(0d));

                Assert.That(bottom.Value, Is.LessThanOrEqualTo(Max));
                Assert.That(bottom.Value, Is.GreaterThanOrEqualTo(Min));
                Assert.That(right.Value, Is.LessThanOrEqualTo(Max));
                Assert.That(right.Value, Is.GreaterThanOrEqualTo(Min));
            }
        }

        [Test]
        public void required_edit_vars()
        {
            var x = new ClVariable("x");
            var y = new ClVariable("y");
            var w = new ClVariable("w");
            var h = new ClVariable("h");

            var target = GetTarget();

            target.AddStay(x);
            target.AddStay(y);
            target.AddStay(w);
            target.AddStay(h);

            target.AddEditVar(x);
            target.AddEditVar(y);

            target.BeginEdit();
            target.SuggestValue(x, 10d);
            target.SuggestValue(y, 20d);
            target.EndEdit();

            Assert.That(x.Value, IsX.Approx(10d));
            Assert.That(y.Value, IsX.Approx(20d));
            Assert.That(w.Value, IsX.Approx(0d));
            Assert.That(h.Value, IsX.Approx(0d));

            // Open a second set of variables for editing

            target.AddEditVar(w);
            target.AddEditVar(h);

            target.BeginEdit();
            target.SuggestValue(w, 30d);
            target.SuggestValue(h, 40d);
            target.EndEdit();

            Assert.That(x.Value, IsX.Approx(10d));
            Assert.That(y.Value, IsX.Approx(20d));
            Assert.That(w.Value, IsX.Approx(30d));
            Assert.That(h.Value, IsX.Approx(40d));

            // Now make sure the first set can still be edited

            target.AddEditVar(x);
            target.AddEditVar(y);

            target.BeginEdit();
            target.SuggestValue(x, 50d);
            target.SuggestValue(y, 60d);
            target.EndEdit();

            Assert.That(x.Value, IsX.Approx(50d));
            Assert.That(y.Value, IsX.Approx(60d));
            Assert.That(w.Value, IsX.Approx(30d));
            Assert.That(h.Value, IsX.Approx(40d));


        }

        [Test]
        public void test_error_weights()
        {
            var x = new ClVariable("x", 100d);
            var y = new ClVariable("y", 200d);
            var z = new ClVariable("z", 50d);

            var target = GetTarget();

            Assert.That(x.Value, IsX.Approx(100d));
            Assert.That(y.Value, IsX.Approx(200d));
            Assert.That(z.Value, IsX.Approx(50d));

            target.AddConstraint((z == x).WithStrength(ClStrength.Weak));
            target.AddConstraint((x == 20d).WithStrength(ClStrength.Weak));
            target.AddConstraint((y == 200d).WithStrength(ClStrength.Strong));

            Assert.That(x.Value, IsX.Approx(20d));
            Assert.That(y.Value, IsX.Approx(200d));
            Assert.That(z.Value, IsX.Approx(20d));

            target.AddConstraint((z + 150d <= y).WithStrength(ClStrength.Medium));

            Assert.That(x.Value, IsX.Approx(20d));
            Assert.That(y.Value, IsX.Approx(200d));
            Assert.That(z.Value, IsX.Approx(20d));
        }

        private struct Point
        {
            private readonly string id;
            private readonly ClVariable x;
            private readonly ClVariable y;

            public Point(string id, double x, double y)
            {
                this.id = id;
                this.x = new ClVariable(x);
                this.y = new ClVariable(y);
            }

            public ClVariable X
            {
                get { return x; }
            }

            public ClVariable Y
            {
                get { return y; }
            }
        }

        [Test]
        public void test_quadrilateral()
        {
            var allPoints = new[]
            {
                new Point("0", 10d, 10d),
                new Point("1", 10d, 200d),
                new Point("2", 200d, 200d),
                new Point("3", 200d, 10d),

                new Point("m0", 0d, 0d),
                new Point("m1", 0d, 0d),
                new Point("m2", 0d, 0d),
                new Point("m3", 0d, 0d),
            };
            var points = allPoints.Take(4).ToArray();
            var midpoints = allPoints.Skip(4).ToArray();

            var target = GetTarget();

            // different weights so taht we don't satisfy e.g. 
            // x of one point, y of another
            var weight = 1d;
            foreach (var point in points)
            {
                target.AddStay(point.X, ClStrength.Weak, weight);
                target.AddStay(point.Y, ClStrength.Weak, weight);
                weight *= 2d;
            }

            for (int start = 0; start < 4; start++)
            {
                var end = (start + 1) % 4;

                var cleX = (points[start].X + points[end].X) / 2d;
                target.AddConstraint(midpoints[start].X == cleX);

                var cleY = (points[start].Y + points[end].Y) / 2d;
                target.AddConstraint(midpoints[start].Y == cleY);
            }

            var clex0 = points[0].X + 20d;
            target.AddConstraint(clex0 <= points[2].X);
            target.AddConstraint(clex0 <= points[3].X);

            var clex1 = points[1].X + 20d;
            target.AddConstraint(clex1 <= points[2].X);
            target.AddConstraint(clex1 <= points[3].X);

            var cley0 = points[0].Y + 20d;
            target.AddConstraint(cley0 <= points[1].Y);
            target.AddConstraint(cley0 <= points[2].Y);

            var cley3 = points[3].Y + 20d;
            target.AddConstraint(cley3 <= points[1].Y);
            target.AddConstraint(cley3 <= points[2].Y);

            foreach (var point in allPoints)
            {
                target.AddConstraint(point.X >= 0d);
                target.AddConstraint(point.Y >= 0d);
                target.AddConstraint(point.X <= 500d);
                target.AddConstraint(point.Y <= 500d);
            }

            // check the initial answers
            Assert.That(allPoints[0].X.Value, IsX.Approx(10d));
            Assert.That(allPoints[0].Y.Value, IsX.Approx(10d));
            Assert.That(allPoints[1].X.Value, IsX.Approx(10d));
            Assert.That(allPoints[1].Y.Value, IsX.Approx(200d));
            Assert.That(allPoints[2].X.Value, IsX.Approx(200d));
            Assert.That(allPoints[2].Y.Value, IsX.Approx(200d));
            Assert.That(allPoints[3].X.Value, IsX.Approx(200d));
            Assert.That(allPoints[3].Y.Value, IsX.Approx(10d));

            Assert.That(allPoints[4].X.Value, IsX.Approx(10d));
            Assert.That(allPoints[4].Y.Value, IsX.Approx(105d));
            Assert.That(allPoints[5].X.Value, IsX.Approx(105d));
            Assert.That(allPoints[5].Y.Value, IsX.Approx(200d));
            Assert.That(allPoints[6].X.Value, IsX.Approx(200d));
            Assert.That(allPoints[6].Y.Value, IsX.Approx(105d));
            Assert.That(allPoints[7].X.Value, IsX.Approx(105d));
            Assert.That(allPoints[7].Y.Value, IsX.Approx(10d));

            // now move point 2 to  a new location

            target.AddEditVar(points[2].X);
            target.AddEditVar(points[2].Y);

            target.BeginEdit();
            target.SuggestValue(points[2].X, 300d);
            target.SuggestValue(points[2].Y, 400d);
            target.EndEdit();

            // check that the other points have been moved
            Assert.That(allPoints[0].X.Value, IsX.Approx(10d));
            Assert.That(allPoints[0].Y.Value, IsX.Approx(10d));
            Assert.That(allPoints[1].X.Value, IsX.Approx(10d));
            Assert.That(allPoints[1].Y.Value, IsX.Approx(200d));
            Assert.That(allPoints[2].X.Value, IsX.Approx(300d));
            Assert.That(allPoints[2].Y.Value, IsX.Approx(400d));
            Assert.That(allPoints[3].X.Value, IsX.Approx(200d));
            Assert.That(allPoints[3].Y.Value, IsX.Approx(10d));

            Assert.That(allPoints[4].X.Value, IsX.Approx(10d));
            Assert.That(allPoints[4].Y.Value, IsX.Approx(105d));
            Assert.That(allPoints[5].X.Value, IsX.Approx(155d));
            Assert.That(allPoints[5].Y.Value, IsX.Approx(300d));
            Assert.That(allPoints[6].X.Value, IsX.Approx(250d));
            Assert.That(allPoints[6].Y.Value, IsX.Approx(205d));
            Assert.That(allPoints[7].X.Value, IsX.Approx(105d));
            Assert.That(allPoints[7].Y.Value, IsX.Approx(10d));
        }

        private struct Button
        {
            private readonly string id;
            private readonly ClVariable left;
            private readonly ClVariable width;

            public Button(string id)
            {
                this.id = id;
                this.left = new ClVariable("left" + id, 0d);
                this.width = new ClVariable("width" + id, 0d);
            }

            public ClVariable Left
            {
                get { return left; }
            }

            public ClVariable Width
            {
                get { return width; }
            }
        }

        [Test]
        public void test_buttons()
        {
            var b1 = new Button("b1");
            var b2 = new Button("b2");

            var leftLimit = new ClVariable("left", 0d);
            var rightLimit = new ClVariable("right", 0d);

            var target = GetTarget();

            target.AddStay(leftLimit, ClStrength.Required);
            target.AddStay(rightLimit, ClStrength.Weak);

            // The two buttons are the same width
            target.AddConstraint(b1.Width == b2.Width);

            // b1 starts 50 from the left margin.
            target.AddConstraint(b1.Left == leftLimit + 50d);

            // b2 ends 50 from the right margin
            target.AddConstraint(leftLimit + rightLimit == b2.Left + b2.Width + 50d);

            // b2 starts at least 100 from the end of b1
            target.AddConstraint(b2.Left >= b1.Left + b1.Width + 100d);

            // b1 has a minimum width of 87
            target.AddConstraint(b1.Width >= 87d);

            // b1's preferred width is 87
            target.AddConstraint((b1.Width == 87).WithStrength(ClStrength.Strong));

            // b2's minimum width is 113
            target.AddConstraint(b2.Width >= 113d);

            // b2's preferred width is 113
            target.AddConstraint((b2.Width == 113).WithStrength(ClStrength.Strong));

            // Without imposign a stay on the right, 
            // right_limit will be the minimum width for the layout
            Assert.That(b1.Left.Value, IsX.Approx(50d));
            Assert.That(b1.Width.Value, IsX.Approx(113d));
            Assert.That(b2.Left.Value, IsX.Approx(263d));
            Assert.That(b2.Width.Value, IsX.Approx(113d));
            Assert.That(rightLimit.Value, IsX.Approx(426d));

            // The window is 500 pixels wide.
            target.SetEditedValue(rightLimit, 500d);
            var stay1 = new ClStayConstraint(rightLimit, ClStrength.Required);
            target.AddConstraint(stay1);

            Assert.That(b1.Left.Value, IsX.Approx(50d));
            Assert.That(b1.Width.Value, IsX.Approx(113d));
            Assert.That(b2.Left.Value, IsX.Approx(337d));
            Assert.That(b2.Width.Value, IsX.Approx(113d));
            Assert.That(rightLimit.Value, IsX.Approx(500d));

            target.RemoveConstraint(stay1);

            // Expand to 700 pixels
            target.SetEditedValue(rightLimit, 700d);
            var stay2 = new ClStayConstraint(rightLimit, ClStrength.Required);
            target.AddConstraint(stay2);

            Assert.That(b1.Left.Value, IsX.Approx(50d));
            Assert.That(b1.Width.Value, IsX.Approx(113d));
            Assert.That(b2.Left.Value, IsX.Approx(537d));
            Assert.That(b2.Width.Value, IsX.Approx(113d));
            Assert.That(rightLimit.Value, IsX.Approx(700d));

            target.RemoveConstraint(stay2);

            // Contract to 600 pixels
            target.SetEditedValue(rightLimit, 600d);
            var stay3 = new ClStayConstraint(rightLimit, ClStrength.Required);
            target.AddConstraint(stay3);

            Assert.That(b1.Left.Value, IsX.Approx(50d));
            Assert.That(b1.Width.Value, IsX.Approx(113d));
            Assert.That(b2.Left.Value, IsX.Approx(437d));
            Assert.That(b2.Width.Value, IsX.Approx(113d));
            Assert.That(rightLimit.Value, IsX.Approx(600d));

            target.RemoveConstraint(stay3);
        }

        [Test]
        public void test_paper_example()
        {
            var left = new ClVariable("left");
            var middle = new ClVariable("middle");
            var right = new ClVariable("right");

            var target = GetTarget();

            target.AddConstraint(middle == (left + right)/2d);
            target.AddConstraint(right == left + 10d);
            target.AddConstraint(right <= 100d);
            target.AddConstraint(left >= 0d);

            // check that all the required constraints are true
            Assert.That(middle.Value, IsX.Approx((left.Value + right.Value) / 2d));
            Assert.That(right.Value, IsX.Approx(left.Value + 10d));
            Assert.That(right.Value, Is.LessThanOrEqualTo(100d));
            Assert.That(left.Value, Is.GreaterThanOrEqualTo(0d));

            // set the middle value to a stay
            middle.Value = 45d;
            //target.SetEditedValue(middle, 45d);
            
            target.AddStay(middle);

            // check that all the required constraints are true
            Assert.That(middle.Value, IsX.Approx((left.Value + right.Value) / 2d));
            Assert.That(right.Value, IsX.Approx(left.Value + 10d));
            Assert.That(right.Value, Is.LessThanOrEqualTo(100d));
            Assert.That(left.Value, Is.GreaterThanOrEqualTo(0d));

            // but more than that - since we gave a position for middle, we know
            // where all the points should be.
            Assert.That(left.Value, IsX.Approx(40d));
            Assert.That(middle.Value, IsX.Approx(45d));
            Assert.That(right.Value, IsX.Approx(50d));
        }

        [Test]
        public void test_2_buttons()
        {
            var panelLeft = new ClVariable("panel.Left");
            var panelMiddle = new ClVariable("panel.Middle");
            var panelRight = new ClVariable("panel.Right");
            var panelWidth = new ClVariable("panel.Width");

            var button1Left = new ClVariable("button1.Left");
            var button1Middle = new ClVariable("button1.Middle");
            var button1Right = new ClVariable("button1.Right");
            var button1Width = new ClVariable("button1.Width");

            var button2Left = new ClVariable("button2.Left");
            var button2Middle = new ClVariable("button2.Middle");
            var button2Right = new ClVariable("button2.Right");
            var button2Width = new ClVariable("button2.Width");

            var target = GetTarget();

            // left,width >= 0
            target.AddConstraint((panelLeft >= 0d).WithStrength(ClStrength.Required));
            target.AddConstraint((panelWidth >= 0d).WithStrength(ClStrength.Required));
            target.AddConstraint((button1Left >= 0d).WithStrength(ClStrength.Required));
            target.AddConstraint((button1Width >= 0d).WithStrength(ClStrength.Required));
            target.AddConstraint((button2Left >= 0d).WithStrength(ClStrength.Required));
            target.AddConstraint((button2Width >= 0d).WithStrength(ClStrength.Required));

            // panel
            target.AddConstraint((panelLeft == 0d).WithStrength(ClStrength.Required));

            // Middle == Left + 0.5*Width
            // Right == Left + Width
            target.AddConstraint(
                (panelMiddle == panelLeft + 0.5d*panelWidth)
                    .WithStrength(ClStrength.Required));
            target.AddConstraint(
                (panelRight == panelLeft + panelWidth)
                    .WithStrength(ClStrength.Required));
            target.AddConstraint(
                (button1Middle == button1Left + 0.5d*button1Width)
                    .WithStrength(ClStrength.Required));
            target.AddConstraint(
                (button1Right == button1Left + button1Width)
                    .WithStrength(ClStrength.Required));
            target.AddConstraint(
                (button2Middle == button2Left + 0.5d*button2Width)
                    .WithStrength(ClStrength.Required));
            target.AddConstraint(
                (button2Right == button2Left + button2Width)
                    .WithStrength(ClStrength.Required));

            // 'real' constraints
            target.AddConstraint(
                (button1Left == panelLeft + 10d)
                    .WithStrength(ClStrength.Required));
            target.AddConstraint(
                (button2Left == button1Right)
                    .WithStrength(ClStrength.Required));

            // layout arrange pass
            target.AddConstraint((panelWidth == 500d).WithStrength(ClStrength.Required));

            target.AddConstraint((button1Width == 50d).WithStrength(ClStrength.Strong));
            target.AddConstraint((button2Width == 50d).WithStrength(ClStrength.Strong));

            Assert.That(button1Left.Value, IsX.Approx(10d));
            Assert.That(button1Width.Value, IsX.Approx(50d));
            Assert.That(button1Right.Value, IsX.Approx(60d));

            Assert.That(button2Left.Value, IsX.Approx(60d));
            Assert.That(button2Width.Value, IsX.Approx(50d));
            Assert.That(button2Right.Value, IsX.Approx(110d));


        }
    }
}