using NUnit.Framework;
using UnityFinger.Observers;
using UnityEngine;
using System;

namespace UnityFinger.Tests
{
    [TestFixture(TestName = "DragObserver")]
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
        }
    }
}
