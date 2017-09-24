using NUnit.Framework;
using UnityFinger.Observers;
using UnityEngine;
using System;

namespace UnityFinger.Tests
{
    class DragObserverTest : IDragListener
    {
        ObserverTestSet<DragObserver> testSet;

        DragInfo? dragStartInfo;
        DragInfo? dragInfo;
        DragInfo? dragEndInfo;

        [SetUp]
        public void SetUp()
        {
            dragStartInfo = null;
            dragInfo = null;
            dragEndInfo = null;

            testSet = new ObserverTestSet<DragObserver>();
            testSet.SetUp(() => new DragObserver(new TestConfig(), this, false));
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
            Assert.AreEqual(Result.None, testSet.Enumerator.Current);

            testSet.Timer.ElapsedTime = 2.0f;
            testSet.Input.SetPosition(new Vector2(11f, 11f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Result.InAction, testSet.Enumerator.Current);
            Assert.IsTrue(dragStartInfo.HasValue);
            Assert.AreEqual(new Vector2(10f, 10f), dragStartInfo.Value.prevPosition);
            Assert.AreEqual(new Vector2(11f, 11f), dragStartInfo.Value.position);
            Assert.AreEqual(new Vector2(1f, 1f), dragStartInfo.Value.delta);

            testSet.Input.SetPosition(new Vector2(13f, 13f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Result.InAction, testSet.Enumerator.Current);
            Assert.IsTrue(dragInfo.HasValue);
            Assert.AreEqual(new Vector2(11f, 11f), dragInfo.Value.prevPosition);
            Assert.AreEqual(new Vector2(13f, 13f), dragInfo.Value.position);
            Assert.AreEqual(new Vector2(3f, 3f), dragInfo.Value.delta);

            testSet.Input.FingerCount = 0;

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Result.InAction, testSet.Enumerator.Current);
            Assert.IsTrue(dragEndInfo.HasValue);
            Assert.AreEqual(new Vector2(13f, 13f), dragEndInfo.Value.prevPosition);
            Assert.AreEqual(new Vector2(13f, 13f), dragEndInfo.Value.position);
            Assert.AreEqual(new Vector2(3f, 3f), dragEndInfo.Value.delta);

            Assert.IsFalse(testSet.Enumerator.MoveNext());
        }
    }
}
