using System;
using System.Linq;
using CassowaryNET.Constraints;
using CassowaryNET.Variables;
using NUnit.Framework;

namespace CassowaryNET.Tests
{
    [TestFixture]
    public class HugeTest
    {
        [Test]
        public void huge_test_of_doom()
        {
            #region Variables

            ////////////////////////////////////////////////////////////////
            //                 Individual widgets                         // 
            ////////////////////////////////////////////////////////////////
            var update_top = new Variable("update.top", 0);
            var update_bottom = new Variable("update.bottom", 23);
            var update_left = new Variable("update.left", 0);
            var update_right = new Variable("update.right", 75);
            var update_height = new Variable("update.height", 23);
            var update_width = new Variable("update.width", 75);

            var newpost_top = new Variable("newpost.top", 0);
            var newpost_bottom = new Variable("newpost.bottom", 23);
            var newpost_left = new Variable("newpost.left", 0);
            var newpost_right = new Variable("newpost.right", 75);
            var newpost_width = new Variable("newpost.width", 75);
            var newpost_height = new Variable("newpost.height", 23);

            var quit_bottom = new Variable("quit.bottom", 23);
            var quit_right = new Variable("quit.right", 75);
            var quit_height = new Variable("quit.height", 23);
            var quit_width = new Variable("quit.width", 75);
            var quit_left = new Variable("quit.left", 0);
            var quit_top = new Variable("quit.top", 0);

            var l_title_top = new Variable("l_title.top", 0);
            var l_title_bottom = new Variable("l_title.bottom", 23);
            var l_title_left = new Variable("l_title.left", 0);
            var l_title_right = new Variable("l_title.right", 100);
            var l_title_height = new Variable("l_title.height", 23);
            var l_title_width = new Variable("l_title.width", 100);

            var title_top = new Variable("title.top", 0);
            var title_bottom = new Variable("title.bottom", 20);
            var title_left = new Variable("title.left.", 0);
            var title_right = new Variable("title.right", 100);
            var title_height = new Variable("title.height", 20);
            var title_width = new Variable("title.width", 100);

            var l_body_top = new Variable("l_body.top", 0);
            var l_body_bottom = new Variable("l_body.bottom", 23);
            var l_body_left = new Variable("l_body.left", 0);
            var l_body_right = new Variable("l_body.right", 100);
            var l_body_height = new Variable("l_body.height.", 23);
            var l_body_width = new Variable("l_body.width", 100);

            var blogentry_top = new Variable("blogentry.top", 0);
            var blogentry_bottom = new Variable("blogentry.bottom", 315);
            var blogentry_left = new Variable("blogentry.left", 0);
            var blogentry_right = new Variable("blogentry.right", 400);
            var blogentry_height = new Variable("blogentry.height", 315);
            var blogentry_width = new Variable("blogentry.width", 400);

            var l_recent_top = new Variable("l_recent.top", 0);
            var l_recent_bottom = new Variable("l_recent.bottom", 23);
            var l_recent_left = new Variable("l_recent.left", 0);
            var l_recent_right = new Variable("l_recent.right", 100);
            var l_recent_height = new Variable("l_recent.height", 23);
            var l_recent_width = new Variable("l_recent.width", 100);

            var articles_top = new Variable("articles.top", 0);
            var articles_bottom = new Variable("articles.bottom", 415);
            var articles_left = new Variable("articles.left", 0);
            var articles_right = new Variable("articles.right", 180);
            var articles_height = new Variable("articles.height", 415);
            var articles_width = new Variable("articles.width", 100);
            ////////////////////////////////////////////////////////////////
            //                  Container widgets                         // 
            ////////////////////////////////////////////////////////////////
            var topRight_top = new Variable("topRight.top", 0);
            //topRight_top = new ClVariable("topRight.top", 0);
            var topRight_bottom = new Variable("topRight.bottom", 100);
            //topRight_bottom = new ClVariable("topRight.bottom", 100);
            var topRight_left = new Variable("topRight.left", 0);
            //topRight_left = new ClVariable("topRight.left", 0);
            var topRight_right = new Variable("topRight.right", 200);
            //topRight_right = new ClVariable("topRight.right", 200);
            var topRight_height = new Variable("topRight.height", 100);
            //topRight_height = new ClVariable("topRight.height", 100);
            var topRight_width = new Variable("topRight.width", 200);
            //topRight_width = new ClVariable("topRight.width", 200);
            //topRight_width = new ClVariable("topRight.width", 200);

            var bottomRight_top = new Variable("bottomRight.top", 0);
            //bottomRight_top = new ClVariable("bottomRight.top", 0);
            var bottomRight_bottom = new Variable("bottomRight.bottom", 100);
            //bottomRight_bottom = new ClVariable("bottomRight.bottom", 100);
            var bottomRight_left = new Variable("bottomRight.left", 0);
            //bottomRight_left = new ClVariable("bottomRight.left", 0);
            var bottomRight_right = new Variable("bottomRight.right", 200);
            //bottomRight_right = new ClVariable("bottomRight.right", 200);
            var bottomRight_height = new Variable("bottomRight.height", 100);
            //bottomRight_height = new ClVariable("bottomRight.height", 100);
            var bottomRight_width = new Variable("bottomRight.width", 200);
            //bottomRight_width = new ClVariable("bottomRight.width", 200);

            var right_top = new Variable("right.top", 0);
            //right_top = new ClVariable("right.top", 0);
            var right_bottom = new Variable("right.bottom", 100);
            //right_bottom = new ClVariable("right.bottom", 100);
            //right_bottom = new ClVariable("right.bottom", 100);
            var right_left = new Variable("right.left", 0);
            //right_left = new ClVariable("right.left", 0);
            var right_right = new Variable("right.right", 200);
            //right_right = new ClVariable("right.right", 200);
            var right_height = new Variable("right.height", 100);
            //right_height = new ClVariable("right.height", 100);
            var right_width = new Variable("right.width", 200);
            //right_width = new ClVariable("right.width", 200);
            //right_width = new ClVariable("right.width", 200);

            var left_top = new Variable("left.top", 0);
            //left_top = new ClVariable("left.top", 0);
            var left_bottom = new Variable("left.bottom", 100);
            //left_bottom = new ClVariable("left.bottom", 100);
            var left_left = new Variable("left.left", 0);
            //left_left = new ClVariable("left.left", 0);
            var left_right = new Variable("left.right", 200);
            //left_right = new ClVariable("left.right", 200);
            var left_height = new Variable("left.height", 100);
            //left_height = new ClVariable("left.height", 100);
            var left_width = new Variable("left.width", 200);
            //left_width = new ClVariable("left.width", 200);

            var fr_top = new Variable("fr.top", 0);
            var fr_bottom = new Variable("fr.bottom", 100);
            var fr_left = new Variable("fr.left", 0);
            var fr_right = new Variable("fr.right", 200);
            var fr_height = new Variable("fr.height", 100);
            var fr_width = new Variable("fr.width", 200);

            #endregion

            var target = new CassowarySolver(); // GetTarget();

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
            target.AddStay(blogentry_width, Strength.Strong);
            target.AddStay(l_recent_height);
            target.AddStay(l_recent_width);
            target.AddStay(articles_height);
            target.AddStay(articles_width);

            #endregion

            #region Required Constraints

            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(bottomRight_height) + bottomRight_top,
                    new LinearExpression(bottomRight_bottom),
                    Strength.Required));
            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(bottomRight_width) + bottomRight_left,
                    new LinearExpression(bottomRight_right),
                    Strength.Required));
            InequalityType geq = InequalityType.GreaterThanOrEqual;
            target.AddConstraint(
                new InequalityConstraint(
                    bottomRight_top,
                    geq,
                    0,
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(
                    bottomRight_bottom,
                    geq,
                    0,
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(
                    bottomRight_left,
                    geq,
                    0,
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(
                    bottomRight_right,
                    geq,
                    0,
                    Strength.Required));

            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(update_height) + update_top,
                    new LinearExpression(update_bottom),
                    Strength.Required));
            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(update_width) + update_left,
                    new LinearExpression(update_right),
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(update_top, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(update_bottom, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(update_left, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(update_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(update_right, geq, 0, Strength.Required));
            InequalityType leq = InequalityType.LessThanOrEqual;
            target.AddConstraint(
                new InequalityConstraint(update_bottom, leq, bottomRight_height));
            target.AddConstraint(
                new InequalityConstraint(update_right, leq, bottomRight_width));

            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(newpost_height) + newpost_top,
                    new LinearExpression(newpost_bottom),
                    Strength.Required));
            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(newpost_width) + newpost_left,
                    new LinearExpression(newpost_right),
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(newpost_top, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(newpost_bottom, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(newpost_left, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(newpost_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(newpost_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(newpost_bottom, leq, bottomRight_height));
            target.AddConstraint(
                new InequalityConstraint(newpost_right, leq, bottomRight_width));

            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(quit_height) + quit_top,
                    new LinearExpression(quit_bottom),
                    Strength.Required));
            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(quit_width) + quit_left,
                    new LinearExpression(quit_right),
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(quit_top, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(quit_bottom, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(quit_left, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(quit_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(quit_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(quit_bottom, leq, bottomRight_height));
            target.AddConstraint(
                new InequalityConstraint(quit_right, leq, bottomRight_width));

            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(topRight_height) + topRight_top,
                    new LinearExpression(topRight_bottom),
                    Strength.Required));
            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(topRight_width) + topRight_left,
                    new LinearExpression(topRight_right),
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(topRight_top, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(
                    topRight_bottom,
                    geq,
                    0,
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(topRight_left, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(topRight_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(topRight_right, geq, 0, Strength.Required));

            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(l_title_height) + l_title_top,
                    new LinearExpression(l_title_bottom),
                    Strength.Required));
            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(l_title_width) + l_title_left,
                    new LinearExpression(l_title_right),
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_title_top, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_title_bottom, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_title_left, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_title_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_title_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_title_bottom, leq, topRight_height));
            target.AddConstraint(
                new InequalityConstraint(l_title_right, leq, topRight_width));

            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(title_height) + title_top,
                    new LinearExpression(title_bottom),
                    Strength.Required));
            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(title_width) + title_left,
                    new LinearExpression(title_right),
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(title_top, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(title_bottom, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(title_left, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(title_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(title_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(title_bottom, leq, topRight_height));
            target.AddConstraint(
                new InequalityConstraint(title_right, leq, topRight_width));

            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(l_body_height) + l_body_top,
                    new LinearExpression(l_body_bottom),
                    Strength.Required));
            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(l_body_width) + l_body_left,
                    new LinearExpression(l_body_right),
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_body_top, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_body_bottom, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_body_left, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_body_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_body_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_body_bottom, leq, topRight_height));
            target.AddConstraint(
                new InequalityConstraint(l_body_right, leq, topRight_width));

            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(blogentry_height) + blogentry_top,
                    new LinearExpression(blogentry_bottom),
                    Strength.Required));
            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(blogentry_width) + blogentry_left,
                    new LinearExpression(blogentry_right),
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(blogentry_top, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(
                    blogentry_bottom,
                    geq,
                    0,
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(blogentry_left, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(
                    blogentry_right,
                    geq,
                    0,
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(
                    blogentry_right,
                    geq,
                    0,
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(blogentry_bottom, leq, topRight_height));
            target.AddConstraint(
                new InequalityConstraint(blogentry_right, leq, topRight_width));

            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(left_height) + left_top,
                    new LinearExpression(left_bottom),
                    Strength.Required));
            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(left_width) + left_left,
                    new LinearExpression(left_right),
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(left_top, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(left_bottom, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(left_left, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(left_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(left_right, geq, 0, Strength.Required));

            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(l_recent_height) + l_recent_top,
                    new LinearExpression(l_recent_bottom),
                    Strength.Required));
            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(l_recent_width) + l_recent_left,
                    new LinearExpression(l_recent_right),
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_recent_top, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(
                    l_recent_bottom,
                    geq,
                    0,
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_recent_left, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_recent_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_recent_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(l_recent_bottom, leq, left_height));
            target.AddConstraint(
                new InequalityConstraint(l_recent_right, leq, left_width));

            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(articles_height) + articles_top,
                    new LinearExpression(articles_bottom),
                    Strength.Required));
            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(articles_width) + articles_left,
                    new LinearExpression(articles_right),
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(articles_top, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(
                    articles_bottom,
                    geq,
                    0,
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(articles_left, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(articles_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(articles_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(articles_bottom, leq, left_height));
            target.AddConstraint(
                new InequalityConstraint(articles_right, leq, left_width));

            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(left_height) + left_top,
                    new LinearExpression(left_bottom),
                    Strength.Required));
            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(left_width) + left_left,
                    new LinearExpression(left_right),
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(left_top, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(left_bottom, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(left_left, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(left_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(left_right, geq, 0, Strength.Required));

            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(right_height) + right_top,
                    new LinearExpression(right_bottom),
                    Strength.Required));
            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(right_width) + right_left,
                    new LinearExpression(right_right),
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(right_top, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(right_bottom, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(right_left, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(right_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(right_right, geq, 0, Strength.Required));

            target.AddConstraint(
                new InequalityConstraint(topRight_bottom, leq, right_height));
            target.AddConstraint(
                new InequalityConstraint(topRight_right, leq, right_width));

            target.AddConstraint(
                new InequalityConstraint(bottomRight_bottom, leq, right_height));
            target.AddConstraint(
                new InequalityConstraint(bottomRight_right, leq, right_width));

            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(fr_height) + fr_top,
                    new LinearExpression(fr_bottom),
                    Strength.Required));
            target.AddConstraint(
                new EqualityConstraint(
                    new LinearExpression(fr_width) + fr_left,
                    new LinearExpression(fr_right),
                    Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(fr_top, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(fr_bottom, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(fr_left, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(fr_right, geq, 0, Strength.Required));
            target.AddConstraint(
                new InequalityConstraint(fr_right, geq, 0, Strength.Required));

            target.AddConstraint(
                new InequalityConstraint(left_bottom, leq, fr_height));
            target.AddConstraint(
                new InequalityConstraint(left_right, leq, fr_width));
            target.AddConstraint(
                new InequalityConstraint(right_bottom, leq, fr_height));
            target.AddConstraint(
                new InequalityConstraint(right_right, leq, fr_width));

            #endregion

            #region Strong Constraints

            target.AddConstraint(
                new InequalityConstraint(
                    update_right,
                    leq,
                    newpost_left,
                    Strength.Strong));
            target.AddConstraint(
                new InequalityConstraint(
                    newpost_right,
                    leq,
                    quit_left,
                    Strength.Strong));
            //_solver.AddConstraint(new ClLinearEquation(bottomRight_width, new ClLinearExpression(topRight_width), ClStrength.Strong));
            //_solver.AddConstraint(new ClLinearEquation(right_width, new ClLinearExpression(topRight_width), ClStrength.Strong));
            target.AddConstraint(
                new EqualityConstraint(
                    bottomRight_bottom,
                    new LinearExpression(right_bottom),
                    Strength.Strong));
            target.AddConstraint(
                new EqualityConstraint(
                    newpost_height,
                    new LinearExpression(update_height),
                    Strength.Strong));
            target.AddConstraint(
                new EqualityConstraint(
                    newpost_width,
                    new LinearExpression(update_width),
                    Strength.Strong));
            target.AddConstraint(
                new EqualityConstraint(
                    update_height,
                    new LinearExpression(quit_height),
                    Strength.Strong));
            target.AddConstraint(
                new EqualityConstraint(
                    quit_width,
                    new LinearExpression(update_width),
                    Strength.Strong));

            target.AddConstraint(
                new InequalityConstraint(
                    l_title_bottom,
                    leq,
                    title_top,
                    Strength.Strong));
            target.AddConstraint(
                new InequalityConstraint(
                    title_bottom,
                    leq,
                    l_body_top,
                    Strength.Strong));
            target.AddConstraint(
                new InequalityConstraint(
                    l_body_bottom,
                    leq,
                    blogentry_top,
                    Strength.Strong));

            target.AddConstraint(
                new EqualityConstraint(
                    title_width,
                    new LinearExpression(blogentry_width),
                    Strength.Strong));

            target.AddConstraint(
                new InequalityConstraint(
                    l_recent_bottom,
                    leq,
                    articles_top,
                    Strength.Strong));

            target.AddConstraint(
                new InequalityConstraint(
                    topRight_bottom,
                    leq,
                    bottomRight_top,
                    Strength.Strong));
            target.AddConstraint(
                new InequalityConstraint(
                    left_right,
                    leq,
                    right_left,
                    Strength.Strong));
            //_solver.AddConstraint(new ClLinearEquation(left_height, new ClLinearExpression(right_height), ClStrength.Strong));
            //_solver.AddConstraint(new ClLinearEquation(fr_height, new ClLinearExpression(right_height), ClStrength.Strong));

            // alignment
            target.AddConstraint(
                new EqualityConstraint(
                    l_title_left,
                    new LinearExpression(title_left),
                    Strength.Strong));
            target.AddConstraint(
                new EqualityConstraint(
                    title_left,
                    new LinearExpression(blogentry_left),
                    Strength.Strong));
            target.AddConstraint(
                new EqualityConstraint(
                    l_body_left,
                    new LinearExpression(blogentry_left),
                    Strength.Strong));
            target.AddConstraint(
                new EqualityConstraint(
                    l_recent_left,
                    new LinearExpression(articles_left),
                    Strength.Strong));

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

            Assert.That(bottomRight_top.Value, IsX.Approx(415d)); //381d
            Assert.That(bottomRight_bottom.Value, IsX.Approx(438d)); //404d
            Assert.That(bottomRight_left.Value, IsX.Approx(175d)); //0d
            Assert.That(bottomRight_right.Value, IsX.Approx(400d)); //225d
            Assert.That(bottomRight_height.Value, IsX.Approx(23d));
            Assert.That(bottomRight_width.Value, IsX.Approx(225d));

            Assert.That(right_top.Value, IsX.Approx(0d));
            Assert.That(right_bottom.Value, IsX.Approx(438d)); //404d
            Assert.That(right_left.Value, IsX.Approx(100d));
            Assert.That(right_right.Value, IsX.Approx(500d));
            Assert.That(right_height.Value, IsX.Approx(438d)); //404d
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