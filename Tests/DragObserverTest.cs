using NUnit.Framework;
using UnityFinger.Factories;
using UnityEngine;
using System;

namespace UnityFinger.Test
{
    class DragObserverTest : IDragListener
    {
        ObserverTestSet<DragObserverFactory> testSet;

        DragInfo? dragStartInfo;
        DragInfo? dragInfo;
        DragInfo? dragEndInfo;

        [SetUp]
        public void SetUp()
        {
            dragStartInfo = null;
            dragInfo = null;
            dragEndInfo = null;

            testSet = new ObserverTestSet<DragObserverFactory>();
            testSet.SetUp(() => new DragObserverFactory(new TestConfig(), this));
        }

        void IDragListener.OnDragStart(DragInfo info)
        {
            dragStartInfo = info;
        }

        void IDragListener.OnDrag(DragInfo info)
        {
            dragInfo = info;
        }

        void IDragListener.OnDragEnd(DragInfo info)
        {
            dragEndInfo = info;
        }

        [Test]
        public void Works()
        {
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
            Assert.AreEqual(new Vector2(10f, 10f), dragStartInfo.Value.Origin);
            Assert.AreEqual(new Vector2(11f, 11f), dragInfo.Value.Previous);
            Assert.AreEqual(new Vector2(13f, 13f), dragInfo.Value.Current);

            testSet.Input.FingerCount = 0;

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.Fired, testSet.Enumerator.Current);
            Assert.IsTrue(dragEndInfo.HasValue);
            Assert.AreEqual(new Vector2(10f, 10f), dragStartInfo.Value.Origin);
            Assert.AreEqual(new Vector2(13f, 13f), dragEndInfo.Value.Previous);
            Assert.AreEqual(new Vector2(13f, 13f), dragEndInfo.Value.Current);

            Assert.IsFalse(testSet.Enumerator.MoveNext());
        }
    }
}
