using System;
using System.Linq;
using Cassowary.Constraints;
using Cassowary.Exceptions;
using Cassowary.Variables;
using NUnit.Framework;

namespace Cassowary.Tests
{
    [TestFixture]
    public class EndToEndTests
    {
        private static readonly Random random = new Random();

        private static ClSimplexSolver GetTarget()
        {
            return new ClSimplexSolver();
        }

        [Test]
        public void can_solve_simple_constraint()
        {
            var x = new ClVariable(167d);
            var y = new ClVariable(2d);
            var target = GetTarget();

            var equation = new ClLinearEquation(x, new ClLinearExpression(y));
            target.AddConstraint(equation);

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

            var constraint = new ClLinearInequality(
                x,
                InequalityType.GreaterThanOrEqual,
                new ClLinearExpression(10d));
            target.AddConstraint(constraint);

            Assert.That(x.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_variable_less_than_constant()
        {
            var x = new ClVariable(10d);
            var target = GetTarget();

            var constraint = new ClLinearInequality(
                x,
                InequalityType.LessThanOrEqual,
                new ClLinearExpression(5d));
            target.AddConstraint(constraint);

            Assert.That(x.Value, IsX.Approx(5d));
        }

        [Test]
        public void can_solve_variable_equal_to_constant()
        {
            var x = new ClVariable(5d);
            var target = GetTarget();

            target.AddConstraint(new ClLinearEquation(x, 10d));

            Assert.That(x.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_constant_greater_than_variable()
        {
            var x = new ClVariable(10d);
            var target = GetTarget();

            var constraint = new ClLinearInequality(
                new ClLinearExpression(5d),
                InequalityType.GreaterThanOrEqual,
                x);
            target.AddConstraint(constraint);

            Assert.That(x.Value, IsX.Approx(5d));
        }

        [Test]
        public void can_solve_constant_less_than_variable()
        {
            var x = new ClVariable(5d);
            var target = GetTarget();

            var constraint = new ClLinearInequality(
                new ClLinearExpression(10d),
                InequalityType.LessThanOrEqual,
                x);
            target.AddConstraint(constraint);

            Assert.That(x.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_greater_than_with_stay()
        {
            var x = new ClVariable(10d);
            var width = new ClVariable(10d);
            var rightMin = new ClVariable(100d);

            var right = new ClLinearExpression(x) + new ClLinearExpression(width);

            var target = GetTarget();

            // right >= 100
            var constraint = new ClLinearInequality(
                right,
                InequalityType.GreaterThanOrEqual,
                rightMin);

            target.AddStay(width);
            target.AddStay(rightMin);
            target.AddConstraint(constraint);

            Assert.That(x.Value, IsX.Approx(90d));
            Assert.That(width.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_less_than_with_stay()
        {
            var x = new ClVariable(200d);
            var width = new ClVariable(10d);
            var rightMin = new ClVariable(100d);

            var right = new ClLinearExpression(x) + new ClLinearExpression(width);

            var target = GetTarget();

            // right <= 100
            var constraint = new ClLinearInequality(
                right,
                InequalityType.LessThanOrEqual,
                rightMin);

            target.AddStay(width);
            target.AddStay(rightMin);
            target.AddConstraint(constraint);

            Assert.That(x.Value, IsX.Approx(90d));
            Assert.That(width.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_equal_to_with_stay()
        {
            var x = new ClVariable(10d);
            var width = new ClVariable(10d);
            var rightMin = new ClVariable(100d);

            var right = new ClLinearExpression(x) + new ClLinearExpression(width);

            var target = GetTarget();

            var constraint = new ClLinearEquation(
                right,
                rightMin);

            target.AddStay(width);
            target.AddStay(rightMin);
            target.AddConstraint(constraint);

            Assert.That(x.Value, IsX.Approx(90d));
            Assert.That(width.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_greater_than_with_variable()
        {
            var x = new ClVariable(10d);
            var width = new ClVariable(10d);
            var rightMin = new ClVariable(100d);

            var right = new ClLinearExpression(x) + new ClLinearExpression(width);

            var target = GetTarget();

            // right >= 100
            var constraint = new ClLinearInequality(
                right,
                InequalityType.GreaterThanOrEqual,
                rightMin);

            target.AddStay(width);
            target.AddStay(rightMin);
            target.AddConstraint(constraint);

            Assert.That(x.Value, IsX.Approx(90d));
            Assert.That(width.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_less_than_with_variable()
        {
            var x = new ClVariable(200d);
            var width = new ClVariable(10d);
            var rightMin = new ClVariable(100d);

            var right = new ClLinearExpression(x) + new ClLinearExpression(width);

            var target = GetTarget();

            // right <= 100
            var constraint = new ClLinearInequality(
                right,
                InequalityType.LessThanOrEqual,
                rightMin);

            target.AddStay(width);
            target.AddStay(rightMin);
            target.AddConstraint(constraint);

            Assert.That(x.Value, IsX.Approx(90d));
            Assert.That(width.Value, IsX.Approx(10d));
        }

        [Test]
        public void can_solve_equal_to_with_expression()
        {
            var x1 = new ClVariable(10d);
            var width1 = new ClVariable(10d);
            var right1 = new ClLinearExpression(x1) + new ClLinearExpression(width1);

            var x2 = new ClVariable(100d);
            var width2 = new ClVariable(10d);
            var right2 = new ClLinearExpression(x2) + new ClLinearExpression(width2);

            var target = GetTarget();

            var constraint = new ClLinearEquation(
                right1,
                right2);

            target.AddStay(width1);
            target.AddStay(width2);
            target.AddStay(x2);
            target.AddConstraint(constraint);

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
            var right1 = new ClLinearExpression(x1) + new ClLinearExpression(width1);

            var x2 = new ClVariable(100d);
            var width2 = new ClVariable(10d);
            var right2 = new ClLinearExpression(x2) + new ClLinearExpression(width2);

            var target = GetTarget();

            var constraint = new ClLinearInequality(
                right1,
                InequalityType.GreaterThanOrEqual,
                right2);

            target.AddStay(width1);
            target.AddStay(width2);
            target.AddStay(x2);
            target.AddConstraint(constraint);

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
            var right2 = new ClLinearExpression(x2) + new ClLinearExpression(width2);

            var target = GetTarget();

            var constraint = new ClLinearInequality(
                right2,
                InequalityType.LessThanOrEqual,
                right1);

            target.AddStay(width1);
            target.AddStay(width2);
            target.AddStay(x2);
            target.AddConstraint(constraint);

            Assert.That(x1.Value, IsX.Approx(100d));
        }

        [Test]
        public void add_and_remove()
        {
            var x = new ClVariable("x");
            var target = GetTarget();

            var c100Weak = new ClLinearEquation(x, 100d, ClStrength.Weak);
            target.AddConstraint(c100Weak);

            var c10 = new ClLinearInequality(x, InequalityType.LessThanOrEqual, 10d);
            var c20 = new ClLinearInequality(x, InequalityType.LessThanOrEqual, 20d);
            target.AddConstraint(c10);
            target.AddConstraint(c20);

            Assert.That(x.Value, IsX.Approx(10d));

            target.RemoveConstraint(c10);
            Assert.That(x.Value, IsX.Approx(20d));

            target.RemoveConstraint(c20);
            Assert.That(x.Value, IsX.Approx(100d));

            var c10Again = new ClLinearInequality(x, InequalityType.LessThanOrEqual, 10d);
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

            var cx100Weak = new ClLinearEquation(x, 100d, ClStrength.Weak);
            target.AddConstraint(cx100Weak);
            var cy120Strong = new ClLinearEquation(y, 120d, ClStrength.Strong);
            target.AddConstraint(cy120Strong);

            var c10 = new ClLinearInequality(x, InequalityType.LessThanOrEqual, 10d);
            var c20 = new ClLinearInequality(x, InequalityType.LessThanOrEqual, 20d);
            target.AddConstraint(c10);
            target.AddConstraint(c20);
            Assert.That(x.Value, IsX.Approx(10d));
            Assert.That(y.Value, IsX.Approx(120d));

            target.RemoveConstraint(c10);
            Assert.That(x.Value, IsX.Approx(20d));
            Assert.That(y.Value, IsX.Approx(120d));

            var cxy = new ClLinearEquation(2d*x, y);
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

            target.AddConstraint(new ClLinearInequality(x, InequalityType.LessThanOrEqual, y));
            target.AddConstraint(new ClLinearEquation(y, x + 3d));
            target.AddConstraint(new ClLinearEquation(x, 10d, ClStrength.Weak));
            target.AddConstraint(new ClLinearEquation(y, 10d, ClStrength.Weak));

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

            target.AddConstraint(new ClLinearEquation(x, 10d));

            Assert.That(
                () => target.AddConstraint(new ClLinearEquation(x, 5d)),
                Throws.TypeOf<CassowaryRequiredConstraintFailureException>());
        }

        [Test]
        public void inconsistent2()
        {
            var x = new ClVariable("x");
            var target = GetTarget();

            target.AddConstraint(new ClLinearInequality(x, InequalityType.GreaterThanOrEqual, 10d));

            Assert.That(
                () => target.AddConstraint(
                    new ClLinearInequality(x, InequalityType.LessThanOrEqual, 5d)),
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

            target.AddConstraint(new ClLinearInequality(w, InequalityType.GreaterThanOrEqual, 10d));
            target.AddConstraint(new ClLinearInequality(x, InequalityType.GreaterThanOrEqual, w));
            target.AddConstraint(new ClLinearInequality(y, InequalityType.GreaterThanOrEqual, x));
            target.AddConstraint(new ClLinearInequality(z, InequalityType.GreaterThanOrEqual, y));
            target.AddConstraint(new ClLinearInequality(z, InequalityType.GreaterThanOrEqual, 8d));

            Assert.That(
                () => target.AddConstraint(
                    new ClLinearInequality(z, InequalityType.LessThanOrEqual, 4d)),
                Throws.TypeOf<CassowaryRequiredConstraintFailureException>());
        }

        [Test]
        public void inconsistent4()
        {
            var x = new ClVariable("x");
            var y = new ClVariable("y");
            var target = GetTarget();

            target.AddConstraint(new ClLinearEquation(x, 10d));
            target.AddConstraint(new ClLinearEquation(x, new ClLinearExpression(y)));

            Assert.That(
                () => target.AddConstraint(
                    new ClLinearEquation(y, 5d)),
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
                new ClLinearEquation(
                    width,
                    new ClLinearExpression(iw),
                    ClStrength.Strong,
                    0d));
            target.AddConstraint(
                new ClLinearEquation(
                    height,
                    new ClLinearExpression(ih),
                    ClStrength.Strong,
                    0d));

            target.AddConstraint(
                new ClLinearEquation(
                    top,
                    0d,
                    ClStrength.Weak,
                    0d));
            target.AddConstraint(
                new ClLinearEquation(
                    left,
                    0d,
                    ClStrength.Weak,
                    0d));

            // right is at least left + width
            target.AddConstraint(
                new ClLinearEquation(
                    bottom,
                    top + height,
                    ClStrength.Medium,
                    0d));
            target.AddConstraint(
                new ClLinearEquation(
                    right,
                    left + width,
                    ClStrength.Medium,
                    0d));

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

            target.AddConstraint(
                new ClLinearEquation(z, new ClLinearExpression(x), ClStrength.Weak));
            target.AddConstraint(new ClLinearEquation(x, 20d, ClStrength.Weak));
            target.AddConstraint(new ClLinearEquation(y, 200d, ClStrength.Strong));

            Assert.That(x.Value, IsX.Approx(20d));
            Assert.That(y.Value, IsX.Approx(200d));
            Assert.That(z.Value, IsX.Approx(20d));

            // z + 150 <= y
            target.AddConstraint(
                new ClLinearInequality(
                    z + 150d,
                    InequalityType.LessThanOrEqual,
                    y,
                    ClStrength.Medium));

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

                // (points[start].X + points[end].X) / 2
                var cleX = (points[start].X + points[end].X) / 2d;
                var cleXq = new ClLinearEquation(midpoints[start].X, cleX);
                target.AddConstraint(cleXq);

                var cleY = (points[start].Y + points[end].Y) / 2d;
                var cleYq = new ClLinearEquation(midpoints[start].Y, cleY);
                target.AddConstraint(cleYq);
            }

            var clex0 = points[0].X + 20d;
            var clex02 = new ClLinearInequality(clex0, InequalityType.LessThanOrEqual, points[2].X);
            var clex03 = new ClLinearInequality(clex0, InequalityType.LessThanOrEqual, points[3].X);
            target.AddConstraint(clex02);
            target.AddConstraint(clex03);

            var clex1 = points[1].X + 20d;
            var clex12 = new ClLinearInequality(clex1, InequalityType.LessThanOrEqual, points[2].X);
            var clex13 = new ClLinearInequality(clex1, InequalityType.LessThanOrEqual, points[3].X);
            target.AddConstraint(clex12);
            target.AddConstraint(clex13);

            var cley0 = points[0].Y + 20d;
            var cley01 = new ClLinearInequality(cley0, InequalityType.LessThanOrEqual, points[1].Y);
            var cley02 = new ClLinearInequality(cley0, InequalityType.LessThanOrEqual, points[2].Y);
            target.AddConstraint(cley01);
            target.AddConstraint(cley02);

            var cley3 = points[3].Y + 20d;
            var cley31 = new ClLinearInequality(cley3, InequalityType.LessThanOrEqual, points[1].Y);
            var cley32 = new ClLinearInequality(cley3, InequalityType.LessThanOrEqual, points[2].Y);
            target.AddConstraint(cley31);
            target.AddConstraint(cley32);

            foreach (var point in allPoints)
            {
                target.AddConstraint(
                    new ClLinearInequality(point.X, InequalityType.GreaterThanOrEqual, 0d));
                target.AddConstraint(
                    new ClLinearInequality(point.Y, InequalityType.GreaterThanOrEqual, 0d));
                target.AddConstraint(
                    new ClLinearInequality(point.X, InequalityType.LessThanOrEqual, 500d));
                target.AddConstraint(
                    new ClLinearInequality(point.Y, InequalityType.LessThanOrEqual, 500d));
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
            //solver.add_constraint(b1.width == b2.width)
            target.AddConstraint(
                new ClLinearEquation(
                    b1.Width,
                    new ClLinearExpression(b2.Width)));

            // b1 starts 50 from the left margin.
            //solver.add_constraint(b1.left == left_limit + 50)
            target.AddConstraint(
                new ClLinearEquation(
                    b1.Left,
                    leftLimit + 50d));

            // b2 ends 50 from the right margin
            //solver.add_constraint(left_limit + right_limit == b2.left + b2.width + 50)
            target.AddConstraint(
                new ClLinearEquation(
                    leftLimit + rightLimit,
                    b2.Left + b2.Width + 50d));

            // b2 starts at least 100 from the end of b1
            //solver.add_constraint(b2.left >= (b1.left + b1.width + 100))
            target.AddConstraint(
                new ClLinearInequality(
                    b2.Left,
                    InequalityType.GreaterThanOrEqual,
                    b1.Left + b1.Width + 100d));

            // b1 has a minimum width of 87
            //solver.add_constraint(b1.width >= 87)
            target.AddConstraint(
                new ClLinearInequality(
                    b1.Width,
                    InequalityType.GreaterThanOrEqual,
                    87d));

            // b1's preferred width is 87
            //solver.add_constraint(b1.width == 87, STRONG)
            target.AddConstraint(
                new ClLinearEquation(b1.Width, 87d, ClStrength.Strong));

            // b2's minimum width is 113
            //solver.add_constraint(b2.width >= 113)
            target.AddConstraint(
                new ClLinearInequality(
                    b2.Width,
                    InequalityType.GreaterThanOrEqual,
                    113d));

            // b2's preferred width is 113
            //solver.add_constraint(b2.width == 113, STRONG)
            target.AddConstraint(
                new ClLinearEquation(b2.Width, 113d, ClStrength.Strong));

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

            // middle == (left + right) / 2
            target.AddConstraint(
                new ClLinearEquation(
                    middle,
                    (left + right) / 2d));
            // right == left + 10
            target.AddConstraint(
                new ClLinearEquation(
                    right,
                    left + 10d));
            // right <= 100
            target.AddConstraint(
                new ClLinearInequality(
                    right,
                    InequalityType.LessThanOrEqual,
                    100d));
            // left >= 0
            target.AddConstraint(
                new ClLinearInequality(
                    left,
                    InequalityType.GreaterThanOrEqual,
                    0d));

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
        public void huge_test_of_doom()
        {
            #region Variables

            ////////////////////////////////////////////////////////////////
            //                 Individual widgets                         // 
            ////////////////////////////////////////////////////////////////
            var update_top = new ClVariable("update.top", 0);
            var update_bottom = new ClVariable("update.bottom", 23);
            var update_left = new ClVariable("update.left", 0);
            var update_right = new ClVariable("update.right", 75);
            var update_height = new ClVariable("update.height", 23);
            var update_width = new ClVariable("update.width", 75);

            var newpost_top = new ClVariable("newpost.top", 0);
            var newpost_bottom = new ClVariable("newpost.bottom", 23);
            var newpost_left = new ClVariable("newpost.left", 0);
            var newpost_right = new ClVariable("newpost.right", 75);
            var newpost_width = new ClVariable("newpost.width", 75);
            var newpost_height = new ClVariable("newpost.height", 23);

            var quit_bottom = new ClVariable("quit.bottom", 23);
            var quit_right = new ClVariable("quit.right", 75);
            var quit_height = new ClVariable("quit.height", 23);
            var quit_width = new ClVariable("quit.width", 75);
            var quit_left = new ClVariable("quit.left", 0);
            var quit_top = new ClVariable("quit.top", 0);

            var l_title_top = new ClVariable("l_title.top", 0);
            var l_title_bottom = new ClVariable("l_title.bottom", 23);
            var l_title_left = new ClVariable("l_title.left", 0);
            var l_title_right = new ClVariable("l_title.right", 100);
            var l_title_height = new ClVariable("l_title.height", 23);
            var l_title_width = new ClVariable("l_title.width", 100);

            var title_top = new ClVariable("title.top", 0);
            var title_bottom = new ClVariable("title.bottom", 20);
            var title_left = new ClVariable("title.left.", 0);
            var title_right = new ClVariable("title.right", 100);
            var title_height = new ClVariable("title.height", 20);
            var title_width = new ClVariable("title.width", 100);

            var l_body_top = new ClVariable("l_body.top", 0);
            var l_body_bottom = new ClVariable("l_body.bottom", 23);
            var l_body_left = new ClVariable("l_body.left", 0);
            var l_body_right = new ClVariable("l_body.right", 100);
            var l_body_height = new ClVariable("l_body.height.", 23);
            var l_body_width = new ClVariable("l_body.width", 100);

            var blogentry_top = new ClVariable("blogentry.top", 0);
            var blogentry_bottom = new ClVariable("blogentry.bottom", 315);
            var blogentry_left = new ClVariable("blogentry.left", 0);
            var blogentry_right = new ClVariable("blogentry.right", 400);
            var blogentry_height = new ClVariable("blogentry.height", 315);
            var blogentry_width = new ClVariable("blogentry.width", 400);

            var l_recent_top = new ClVariable("l_recent.top", 0);
            var l_recent_bottom = new ClVariable("l_recent.bottom", 23);
            var l_recent_left = new ClVariable("l_recent.left", 0);
            var l_recent_right = new ClVariable("l_recent.right", 100);
            var l_recent_height = new ClVariable("l_recent.height", 23);
            var l_recent_width = new ClVariable("l_recent.width", 100);

            var articles_top = new ClVariable("articles.top", 0);
            var articles_bottom = new ClVariable("articles.bottom", 415);
            var articles_left = new ClVariable("articles.left", 0);
            var articles_right = new ClVariable("articles.right", 180);
            var articles_height = new ClVariable("articles.height", 415);
            var articles_width = new ClVariable("articles.width", 100);
            ////////////////////////////////////////////////////////////////
            //                  Container widgets                         // 
            ////////////////////////////////////////////////////////////////
            var topRight_top = new ClVariable("topRight.top", 0);
            //topRight_top = new ClVariable("topRight.top", 0);
            var topRight_bottom = new ClVariable("topRight.bottom", 100);
            //topRight_bottom = new ClVariable("topRight.bottom", 100);
            var topRight_left = new ClVariable("topRight.left", 0);
            //topRight_left = new ClVariable("topRight.left", 0);
            var topRight_right = new ClVariable("topRight.right", 200);
            //topRight_right = new ClVariable("topRight.right", 200);
            var topRight_height = new ClVariable("topRight.height", 100);
            //topRight_height = new ClVariable("topRight.height", 100);
            var topRight_width = new ClVariable("topRight.width", 200);
            //topRight_width = new ClVariable("topRight.width", 200);
            //topRight_width = new ClVariable("topRight.width", 200);

            var bottomRight_top = new ClVariable("bottomRight.top", 0);
            //bottomRight_top = new ClVariable("bottomRight.top", 0);
            var bottomRight_bottom = new ClVariable("bottomRight.bottom", 100);
            //bottomRight_bottom = new ClVariable("bottomRight.bottom", 100);
            var bottomRight_left = new ClVariable("bottomRight.left", 0);
            //bottomRight_left = new ClVariable("bottomRight.left", 0);
            var bottomRight_right = new ClVariable("bottomRight.right", 200);
            //bottomRight_right = new ClVariable("bottomRight.right", 200);
            var bottomRight_height = new ClVariable("bottomRight.height", 100);
            //bottomRight_height = new ClVariable("bottomRight.height", 100);
            var bottomRight_width = new ClVariable("bottomRight.width", 200);
            //bottomRight_width = new ClVariable("bottomRight.width", 200);

            var right_top = new ClVariable("right.top", 0);
            //right_top = new ClVariable("right.top", 0);
            var right_bottom = new ClVariable("right.bottom", 100);
            //right_bottom = new ClVariable("right.bottom", 100);
            //right_bottom = new ClVariable("right.bottom", 100);
            var right_left = new ClVariable("right.left", 0);
            //right_left = new ClVariable("right.left", 0);
            var right_right = new ClVariable("right.right", 200);
            //right_right = new ClVariable("right.right", 200);
            var right_height = new ClVariable("right.height", 100);
            //right_height = new ClVariable("right.height", 100);
            var right_width = new ClVariable("right.width", 200);
            //right_width = new ClVariable("right.width", 200);
            //right_width = new ClVariable("right.width", 200);

            var left_top = new ClVariable("left.top", 0);
            //left_top = new ClVariable("left.top", 0);
            var left_bottom = new ClVariable("left.bottom", 100);
            //left_bottom = new ClVariable("left.bottom", 100);
            var left_left = new ClVariable("left.left", 0);
            //left_left = new ClVariable("left.left", 0);
            var left_right = new ClVariable("left.right", 200);
            //left_right = new ClVariable("left.right", 200);
            var left_height = new ClVariable("left.height", 100);
            //left_height = new ClVariable("left.height", 100);
            var left_width = new ClVariable("left.width", 200);
            //left_width = new ClVariable("left.width", 200);

            var fr_top = new ClVariable("fr.top", 0);
            var fr_bottom = new ClVariable("fr.bottom", 100);
            var fr_left = new ClVariable("fr.left", 0);
            var fr_right = new ClVariable("fr.right", 200);
            var fr_height = new ClVariable("fr.height", 100);
            var fr_width = new ClVariable("fr.width", 200);

            #endregion

            var target = new ClSimplexSolver(); // GetTarget();

            #region Stay Constraints

            target.AddStay(update_height);
            target.AddStay(update_width);
            target.AddStay(newpost_height);
            target.AddStay(newpost_width);
            target.AddStay(quit_height);
            target.AddStay(quit_width);
            target.AddStay(l_title_height);
            target.AddStay(l_title_width);
            target.AddStay(title_height);
            target.AddStay(title_width);
            target.AddStay(l_body_height);
            target.AddStay(l_body_width);
            target.AddStay(blogentry_height);
            // let's keep blogentry.width in favor of other stay constraints!
            // remember we later specify title.width to be equal to blogentry.width
            target.AddStay(blogentry_width, ClStrength.Strong);
            target.AddStay(l_recent_height);
            target.AddStay(l_recent_width);
            target.AddStay(articles_height);
            target.AddStay(articles_width);

            #endregion

            #region Required Constraints

            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(bottomRight_height) + bottomRight_top,
                    new ClLinearExpression(bottomRight_bottom),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(bottomRight_width) + bottomRight_left,
                    new ClLinearExpression(bottomRight_right),
                    ClStrength.Required));
            InequalityType geq = InequalityType.GreaterThanOrEqual;
            target.AddConstraint(
                new ClLinearInequality(
                    bottomRight_top,
                    geq,
                    0,
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(
                    bottomRight_bottom,
                    geq,
                    0,
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(
                    bottomRight_left,
                    geq,
                    0,
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(
                    bottomRight_right,
                    geq,
                    0,
                    ClStrength.Required));

            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(update_height) + update_top,
                    new ClLinearExpression(update_bottom),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(update_width) + update_left,
                    new ClLinearExpression(update_right),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(update_top, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(update_bottom, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(update_left, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(update_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(update_right, geq, 0, ClStrength.Required));
            InequalityType leq = InequalityType.LessThanOrEqual;
            target.AddConstraint(
                new ClLinearInequality(update_bottom, leq, bottomRight_height));
            target.AddConstraint(
                new ClLinearInequality(update_right, leq, bottomRight_width));

            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(newpost_height) + newpost_top,
                    new ClLinearExpression(newpost_bottom),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(newpost_width) + newpost_left,
                    new ClLinearExpression(newpost_right),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(newpost_top, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(newpost_bottom, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(newpost_left, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(newpost_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(newpost_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(newpost_bottom, leq, bottomRight_height));
            target.AddConstraint(
                new ClLinearInequality(newpost_right, leq, bottomRight_width));

            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(quit_height) + quit_top,
                    new ClLinearExpression(quit_bottom),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(quit_width) + quit_left,
                    new ClLinearExpression(quit_right),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(quit_top, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(quit_bottom, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(quit_left, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(quit_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(quit_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(quit_bottom, leq, bottomRight_height));
            target.AddConstraint(
                new ClLinearInequality(quit_right, leq, bottomRight_width));

            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(topRight_height) + topRight_top,
                    new ClLinearExpression(topRight_bottom),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(topRight_width) + topRight_left,
                    new ClLinearExpression(topRight_right),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(topRight_top, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(
                    topRight_bottom,
                    geq,
                    0,
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(topRight_left, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(topRight_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(topRight_right, geq, 0, ClStrength.Required));

            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(l_title_height) + l_title_top,
                    new ClLinearExpression(l_title_bottom),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(l_title_width) + l_title_left,
                    new ClLinearExpression(l_title_right),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_title_top, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_title_bottom, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_title_left, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_title_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_title_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_title_bottom, leq, topRight_height));
            target.AddConstraint(
                new ClLinearInequality(l_title_right, leq, topRight_width));

            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(title_height) + title_top,
                    new ClLinearExpression(title_bottom),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(title_width) + title_left,
                    new ClLinearExpression(title_right),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(title_top, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(title_bottom, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(title_left, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(title_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(title_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(title_bottom, leq, topRight_height));
            target.AddConstraint(
                new ClLinearInequality(title_right, leq, topRight_width));

            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(l_body_height) + l_body_top,
                    new ClLinearExpression(l_body_bottom),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(l_body_width) + l_body_left,
                    new ClLinearExpression(l_body_right),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_body_top, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_body_bottom, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_body_left, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_body_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_body_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_body_bottom, leq, topRight_height));
            target.AddConstraint(
                new ClLinearInequality(l_body_right, leq, topRight_width));

            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(blogentry_height) + blogentry_top,
                    new ClLinearExpression(blogentry_bottom),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(blogentry_width) + blogentry_left,
                    new ClLinearExpression(blogentry_right),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(blogentry_top, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(
                    blogentry_bottom,
                    geq,
                    0,
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(blogentry_left, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(
                    blogentry_right,
                    geq,
                    0,
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(
                    blogentry_right,
                    geq,
                    0,
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(blogentry_bottom, leq, topRight_height));
            target.AddConstraint(
                new ClLinearInequality(blogentry_right, leq, topRight_width));

            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(left_height) + left_top,
                    new ClLinearExpression(left_bottom),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(left_width) + left_left,
                    new ClLinearExpression(left_right),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(left_top, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(left_bottom, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(left_left, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(left_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(left_right, geq, 0, ClStrength.Required));

            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(l_recent_height) + l_recent_top,
                    new ClLinearExpression(l_recent_bottom),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(l_recent_width) + l_recent_left,
                    new ClLinearExpression(l_recent_right),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_recent_top, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(
                    l_recent_bottom,
                    geq,
                    0,
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_recent_left, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_recent_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_recent_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(l_recent_bottom, leq, left_height));
            target.AddConstraint(
                new ClLinearInequality(l_recent_right, leq, left_width));

            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(articles_height) + articles_top,
                    new ClLinearExpression(articles_bottom),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(articles_width) + articles_left,
                    new ClLinearExpression(articles_right),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(articles_top, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(
                    articles_bottom,
                    geq,
                    0,
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(articles_left, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(articles_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(articles_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(articles_bottom, leq, left_height));
            target.AddConstraint(
                new ClLinearInequality(articles_right, leq, left_width));

            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(left_height) + left_top,
                    new ClLinearExpression(left_bottom),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(left_width) + left_left,
                    new ClLinearExpression(left_right),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(left_top, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(left_bottom, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(left_left, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(left_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(left_right, geq, 0, ClStrength.Required));

            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(right_height) + right_top,
                    new ClLinearExpression(right_bottom),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(right_width) + right_left,
                    new ClLinearExpression(right_right),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(right_top, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(right_bottom, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(right_left, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(right_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(right_right, geq, 0, ClStrength.Required));

            target.AddConstraint(
                new ClLinearInequality(topRight_bottom, leq, right_height));
            target.AddConstraint(
                new ClLinearInequality(topRight_right, leq, right_width));

            target.AddConstraint(
                new ClLinearInequality(bottomRight_bottom, leq, right_height));
            target.AddConstraint(
                new ClLinearInequality(bottomRight_right, leq, right_width));

            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(fr_height) + fr_top,
                    new ClLinearExpression(fr_bottom),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearEquation(
                    new ClLinearExpression(fr_width) + fr_left,
                    new ClLinearExpression(fr_right),
                    ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(fr_top, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(fr_bottom, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(fr_left, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(fr_right, geq, 0, ClStrength.Required));
            target.AddConstraint(
                new ClLinearInequality(fr_right, geq, 0, ClStrength.Required));

            target.AddConstraint(
                new ClLinearInequality(left_bottom, leq, fr_height));
            target.AddConstraint(
                new ClLinearInequality(left_right, leq, fr_width));
            target.AddConstraint(
                new ClLinearInequality(right_bottom, leq, fr_height));
            target.AddConstraint(
                new ClLinearInequality(right_right, leq, fr_width));

            #endregion

            #region Strong Constraints

            target.AddConstraint(
                new ClLinearInequality(
                    update_right,
                    leq,
                    newpost_left,
                    ClStrength.Strong));
            target.AddConstraint(
                new ClLinearInequality(
                    newpost_right,
                    leq,
                    quit_left,
                    ClStrength.Strong));
            //_solver.AddConstraint(new ClLinearEquation(bottomRight_width, new ClLinearExpression(topRight_width), ClStrength.Strong));
            //_solver.AddConstraint(new ClLinearEquation(right_width, new ClLinearExpression(topRight_width), ClStrength.Strong));
            target.AddConstraint(
                new ClLinearEquation(
                    bottomRight_bottom,
                    new ClLinearExpression(right_bottom),
                    ClStrength.Strong));
            target.AddConstraint(
                new ClLinearEquation(
                    newpost_height,
                    new ClLinearExpression(update_height),
                    ClStrength.Strong));
            target.AddConstraint(
                new ClLinearEquation(
                    newpost_width,
                    new ClLinearExpression(update_width),
                    ClStrength.Strong));
            target.AddConstraint(
                new ClLinearEquation(
                    update_height,
                    new ClLinearExpression(quit_height),
                    ClStrength.Strong));
            target.AddConstraint(
                new ClLinearEquation(
                    quit_width,
                    new ClLinearExpression(update_width),
                    ClStrength.Strong));

            target.AddConstraint(
                new ClLinearInequality(
                    l_title_bottom,
                    leq,
                    title_top,
                    ClStrength.Strong));
            target.AddConstraint(
                new ClLinearInequality(
                    title_bottom,
                    leq,
                    l_body_top,
                    ClStrength.Strong));
            target.AddConstraint(
                new ClLinearInequality(
                    l_body_bottom,
                    leq,
                    blogentry_top,
                    ClStrength.Strong));

            target.AddConstraint(
                new ClLinearEquation(
                    title_width,
                    new ClLinearExpression(blogentry_width),
                    ClStrength.Strong));

            target.AddConstraint(
                new ClLinearInequality(
                    l_recent_bottom,
                    leq,
                    articles_top,
                    ClStrength.Strong));

            target.AddConstraint(
                new ClLinearInequality(
                    topRight_bottom,
                    leq,
                    bottomRight_top,
                    ClStrength.Strong));
            target.AddConstraint(
                new ClLinearInequality(
                    left_right,
                    leq,
                    right_left,
                    ClStrength.Strong));
            //_solver.AddConstraint(new ClLinearEquation(left_height, new ClLinearExpression(right_height), ClStrength.Strong));
            //_solver.AddConstraint(new ClLinearEquation(fr_height, new ClLinearExpression(right_height), ClStrength.Strong));

            // alignment
            target.AddConstraint(
                new ClLinearEquation(
                    l_title_left,
                    new ClLinearExpression(title_left),
                    ClStrength.Strong));
            target.AddConstraint(
                new ClLinearEquation(
                    title_left,
                    new ClLinearExpression(blogentry_left),
                    ClStrength.Strong));
            target.AddConstraint(
                new ClLinearEquation(
                    l_body_left,
                    new ClLinearExpression(blogentry_left),
                    ClStrength.Strong));
            target.AddConstraint(
                new ClLinearEquation(
                    l_recent_left,
                    new ClLinearExpression(articles_left),
                    ClStrength.Strong));

            #endregion

            target.Solve();

            Assert.That(update_top.Value, IsX.Approx(0d));
            Assert.That(update_bottom.Value, IsX.Approx(23d));
            Assert.That(update_left.Value, IsX.Approx(0d));
            Assert.That(update_right.Value, IsX.Approx(75d));
            Assert.That(update_height.Value, IsX.Approx(23d));
            Assert.That(update_width.Value, IsX.Approx(75d));

            Assert.That(newpost_top.Value, IsX.Approx(0d));
            Assert.That(newpost_bottom.Value, IsX.Approx(23d));
            Assert.That(newpost_left.Value, IsX.Approx(75d));
            Assert.That(newpost_right.Value, IsX.Approx(150d));
            Assert.That(newpost_height.Value, IsX.Approx(23d));
            Assert.That(newpost_width.Value, IsX.Approx(75d));

            Assert.That(quit_top.Value, IsX.Approx(0d));
            Assert.That(quit_bottom.Value, IsX.Approx(23d));
            Assert.That(quit_left.Value, IsX.Approx(150d));
            Assert.That(quit_right.Value, IsX.Approx(225d));
            Assert.That(quit_height.Value, IsX.Approx(23d));
            Assert.That(quit_width.Value, IsX.Approx(75d));

            Assert.That(l_title_top.Value, IsX.Approx(0d));
            Assert.That(l_title_bottom.Value, IsX.Approx(23d));
            Assert.That(l_title_left.Value, IsX.Approx(0d));
            Assert.That(l_title_right.Value, IsX.Approx(100d));
            Assert.That(l_title_height.Value, IsX.Approx(23d));
            Assert.That(l_title_width.Value, IsX.Approx(100d));

            Assert.That(title_top.Value, IsX.Approx(23d));
            Assert.That(title_bottom.Value, IsX.Approx(43d));
            Assert.That(title_left.Value, IsX.Approx(0d));
            Assert.That(title_right.Value, IsX.Approx(400d));
            Assert.That(title_height.Value, IsX.Approx(20d));
            Assert.That(title_width.Value, IsX.Approx(400d));

            Assert.That(l_body_top.Value, IsX.Approx(43d));
            Assert.That(l_body_bottom.Value, IsX.Approx(66d));
            Assert.That(l_body_left.Value, IsX.Approx(0d));
            Assert.That(l_body_right.Value, IsX.Approx(100d));
            Assert.That(l_body_height.Value, IsX.Approx(23d));
            Assert.That(l_body_width.Value, IsX.Approx(100d));

            Assert.That(blogentry_top.Value, IsX.Approx(66d));
            Assert.That(blogentry_bottom.Value, IsX.Approx(381d));
            Assert.That(blogentry_left.Value, IsX.Approx(0d));
            Assert.That(blogentry_right.Value, IsX.Approx(400d));
            Assert.That(blogentry_height.Value, IsX.Approx(315d));
            Assert.That(blogentry_width.Value, IsX.Approx(400d));

            Assert.That(l_recent_top.Value, IsX.Approx(0d));
            Assert.That(l_recent_bottom.Value, IsX.Approx(23d));
            Assert.That(l_recent_left.Value, IsX.Approx(0d));
            Assert.That(l_recent_right.Value, IsX.Approx(100d));
            Assert.That(l_recent_height.Value, IsX.Approx(23d));
            Assert.That(l_recent_width.Value, IsX.Approx(100d));

            Assert.That(articles_top.Value, IsX.Approx(23d));
            Assert.That(articles_bottom.Value, IsX.Approx(438d));
            Assert.That(articles_left.Value, IsX.Approx(0d));
            Assert.That(articles_right.Value, IsX.Approx(100d));
            Assert.That(articles_height.Value, IsX.Approx(415d));
            Assert.That(articles_width.Value, IsX.Approx(100d));

            Assert.That(topRight_top.Value, IsX.Approx(0d));
            Assert.That(topRight_bottom.Value, IsX.Approx(381d));
            Assert.That(topRight_left.Value, IsX.Approx(0d));
            Assert.That(topRight_right.Value, IsX.Approx(400d));
            Assert.That(topRight_height.Value, IsX.Approx(381d));
            Assert.That(topRight_width.Value, IsX.Approx(400d));

            Assert.That(bottomRight_top.Value, IsX.Approx(381d));
            Assert.That(bottomRight_bottom.Value, IsX.Approx(404d));
            Assert.That(bottomRight_left.Value, IsX.Approx(175d)); //0d
            Assert.That(bottomRight_right.Value, IsX.Approx(400d)); //225d
            Assert.That(bottomRight_height.Value, IsX.Approx(23d));
            Assert.That(bottomRight_width.Value, IsX.Approx(225d));

            Assert.That(right_top.Value, IsX.Approx(0d));
            Assert.That(right_bottom.Value, IsX.Approx(404d));
            Assert.That(right_left.Value, IsX.Approx(100d));
            Assert.That(right_right.Value, IsX.Approx(500d));
            Assert.That(right_height.Value, IsX.Approx(404d));
            Assert.That(right_width.Value, IsX.Approx(400d));

            Assert.That(left_top.Value, IsX.Approx(0d));
            Assert.That(left_bottom.Value, IsX.Approx(438d));
            Assert.That(left_left.Value, IsX.Approx(0d));
            Assert.That(left_right.Value, IsX.Approx(100d));
            Assert.That(left_height.Value, IsX.Approx(438d));
            Assert.That(left_width.Value, IsX.Approx(100d));

            Assert.That(fr_top.Value, IsX.Approx(0d));
            Assert.That(fr_bottom.Value, IsX.Approx(438d));
            Assert.That(fr_left.Value, IsX.Approx(0d));
            Assert.That(fr_right.Value, IsX.Approx(500d));
            Assert.That(fr_height.Value, IsX.Approx(438d));
            Assert.That(fr_width.Value, IsX.Approx(500d));
        }
    }
}