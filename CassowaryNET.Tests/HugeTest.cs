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