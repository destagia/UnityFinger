using NUnit.Framework;
using UnityFinger.Factories;
using UnityEngine;
using System;

namespace UnityFinger.Test
{
    class DragObserverTest : IDragListener
    {
        DragInfo? dragStartInfo;
        DragInfo? dragInfo;
        bool dragEnd;

        [SetUp]
        public void SetUp()
        {
            dragStartInfo = null;
            dragInfo = null;
            dragEnd = false;
        }

        ObserverTestSet<DragObserverFactory> SetUpTestSet(DragOptionFlag optionFlag)
        {
            var testConfig = new TestConfig();
            testConfig.DragOptionFlag = optionFlag;

            var testSet = new ObserverTestSet<DragObserverFactory>();
            testSet.SetUp(() => new DragObserverFactory(testConfig, this));

            return testSet;
        }

        void IDragListener.OnDragStart(DragInfo info)
        {
            dragStartInfo = info;
        }

        void IDragListener.OnDrag(DragInfo info)
        {
            dragInfo = info;
        }

        void IDragListener.OnDragEnd()
        {
            dragEnd = true;
        }

        [Test]
        public void Works()
        {
            var testSet = SetUpTestSet(DragOptionFlag.None);

            testSet.Input.FingerCount = 1;
            testSet.Input.SetPosition(new Vector2(10f, 10f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);

            testSet.Timer.ElapsedTime = 2.0f;
            testSet.Input.SetPosition(new Vector2(11f, 11f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.Fired, testSet.Enumerator.Current);
            Assert.IsTrue(dragStartInfo.HasValue);
            Assert.AreEqual(new Vector2(10f, 10f), dragStartInfo.Value.Origin);
            Assert.AreEqual(new Vector2(10f, 10f), dragStartInfo.Value.Previous);
            Assert.AreEqual(new Vector2(11f, 11f), dragStartInfo.Value.Current);

            testSet.Input.SetPosition(new Vector2(13f, 13f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.Fired, testSet.Enumerator.Current);
            Assert.IsTrue(dragInfo.HasValue);
            Assert.AreEqual(new Vector2(10f, 10f), dragInfo.Value.Origin);
            Assert.AreEqual(new Vector2(11f, 11f), dragInfo.Value.Previous);
            Assert.AreEqual(new Vector2(13f, 13f), dragInfo.Value.Current);

            testSet.Input.FingerCount = 0;

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.Fired, testSet.Enumerator.Current);
            Assert.IsTrue(dragEnd);

            Assert.IsFalse(testSet.Enumerator.MoveNext());
        }

        [Test]
        public void FailsIfTimeIsNotElapsed()
        {
            var testSet = SetUpTestSet(DragOptionFlag.None);

            testSet.Input.FingerCount = 1;
            testSet.Input.SetPosition(new Vector2(10f, 10f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);

            // finger moved enought to invoke, but time is not enough elpased
            testSet.Timer.ElapsedTime = 0.5f;
            testSet.Input.SetPosition(new Vector2(13f, 13f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);
            Assert.IsFalse(dragStartInfo.HasValue);
        }

        [Test]
        public void FailsIfFingerDoesntMove()
        {
            var testSet = SetUpTestSet(DragOptionFlag.None);

            testSet.Input.FingerCount = 1;
            testSet.Input.SetPosition(new Vector2(10f, 10f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);

            testSet.Timer.ElapsedTime = 2.0f;
            testSet.Input.SetPosition(new Vector2(10f, 10f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);
            Assert.IsFalse(dragStartInfo.HasValue);
        }

        [Test]
        public void WorksIgnoreOthersFlag()
        {
            var testSet = SetUpTestSet(DragOptionFlag.IgnoreOthers);

            testSet.Input.FingerCount = 1;
            testSet.Input.SetPosition(new Vector2(10f, 10f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);

            // time elapsed within DragDuration
            testSet.Timer.ElapsedTime = 0.5f;
            testSet.Input.SetPosition(new Vector2(11f, 11f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.Fired, testSet.Enumerator.Current);
            Assert.IsTrue(dragStartInfo.HasValue);
            Assert.AreEqual(new Vector2(10f, 10f), dragStartInfo.Value.Origin);
            Assert.AreEqual(new Vector2(10f, 10f), dragStartInfo.Value.Previous);
            Assert.AreEqual(new Vector2(11f, 11f), dragStartInfo.Value.Current);
        }

        [Test]
        public void FailsIgnoreOthersFlagIfFingerDoesntMove()
        {
            var testSet = SetUpTestSet(DragOptionFlag.IgnoreOthers);

            testSet.Input.FingerCount = 1;
            testSet.Input.SetPosition(new Vector2(10f, 10f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);

            // time elapsed within DragDuration, and finger didn't move
            testSet.Timer.ElapsedTime = 0.5f;

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);
            Assert.IsFalse(dragStartInfo.HasValue);
        }

        [Test]
        public void WorksImmediateFlag()
        {
            var testSet = SetUpTestSet(DragOptionFlag.Immediate);

            testSet.Input.FingerCount = 1;
            testSet.Input.SetPosition(new Vector2(10f, 10f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);

            // finger has moved slightly
            testSet.Input.SetPosition(new Vector2(10.1f, 10.1f));
            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);

            // time elapsed over DragDuration, and finger has moved slightly again
            testSet.Input.SetPosition(new Vector2(10.2f, 10.2f));
            testSet.Timer.ElapsedTime = 2.0f;

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.Fired, testSet.Enumerator.Current);
            Assert.IsTrue(dragStartInfo.HasValue);
            Assert.AreEqual(new Vector2(10f, 10f), dragStartInfo.Value.Origin);
            Assert.AreEqual(new Vector2(10.1f, 10.1f), dragStartInfo.Value.Previous);
            Assert.AreEqual(new Vector2(10.2f, 10.2f), dragStartInfo.Value.Current);
        }

        [Test]
        public void FailsImmediateFlagIfFingerDoesntMove()
        {
            var testSet = SetUpTestSet(DragOptionFlag.Immediate);

            testSet.Input.FingerCount = 1;
            testSet.Input.SetPosition(new Vector2(10f, 10f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);

            // time elapsed within DragDuration, and finger didn't move
            testSet.Timer.ElapsedTime = 0.5f;

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);
            Assert.IsFalse(dragStartInfo.HasValue);
        }
    }
}
