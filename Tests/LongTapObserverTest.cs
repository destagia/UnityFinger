using NUnit.Framework;
using UnityFinger.Factories;
using UnityEngine;
using System;

namespace UnityFinger.Test
{
    class LongTapObserverTest : ILongTapListener
    {
        ObserverTestSet<LongTapObserverFactory> testSet;

        Vector2? position;

        [SetUp]
        public void SetUp()
        {
            position = null;

            testSet = new ObserverTestSet<LongTapObserverFactory>();
            testSet.SetUp(() => new LongTapObserverFactory(new TestConfig(), this));
        }

        [Test]
        public void Works()
        {
            testSet.Input.FingerCount = 1;
            testSet.Input.SetPosition(new Vector2(5, 5));

            // While tapping, observer is going on.
            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);

            // The duration invoking long tap is 1.0f in TestConfig
            testSet.Timer.ElapsedTime = 2.0f;

            // Long tap is invoked
            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.Fired, testSet.Enumerator.Current);
            Assert.IsTrue(position.HasValue);
            Assert.AreEqual(new Vector2(5, 5), position.Value);

            // observer is finished
            Assert.IsFalse(testSet.Enumerator.MoveNext());
        }

        [Test]
        public void WorksIfFingerMoveLittle()
        {
            testSet.Input.FingerCount = 1;
            testSet.Input.SetPosition(new Vector2(5, 5));

            // While tapping, observer is going on.
            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);

            // finger moved slightly
            testSet.Input.SetPosition(new Vector2(6, 5));

            // The duration invoking long tap is 1.0f in TestConfig
            testSet.Timer.ElapsedTime = 2.0f;

            // Long tap is invoked
            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.Fired, testSet.Enumerator.Current);
            Assert.IsTrue(position.HasValue);
            Assert.AreEqual(new Vector2(6, 5), position.Value);
        }

        [Test]
        public void FailsIfTooEarly()
        {
            testSet.Input.FingerCount = 1;
            testSet.Input.SetPosition(new Vector2(5, 5));

            // While tapping, observer is going on.
            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);

            // observer continues its task
            testSet.Timer.ElapsedTime = 0.2f;
            Assert.IsTrue(testSet.Enumerator.MoveNext());
            testSet.Timer.ElapsedTime = 0.4f;
            Assert.IsTrue(testSet.Enumerator.MoveNext());
            testSet.Timer.ElapsedTime = 0.6f;
            Assert.IsTrue(testSet.Enumerator.MoveNext());

            // the finger is released in early duration (within LongTapDuration)
            testSet.Input.FingerCount = 0;

            // observer stops its task and long tap failed
            Assert.IsFalse(testSet.Enumerator.MoveNext());
            Assert.IsFalse(position.HasValue);
        }

        [Test]
        public void FailsIfFingerMoveMuch()
        {
            testSet.Input.FingerCount = 1;
            testSet.Input.SetPosition(new Vector2(5, 5));

            // While tapping, observer is going on.
            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);

            // finger moved too much
            testSet.Input.SetPosition(new Vector2(100, 100));

            // The duration invoking long tap is 1.0f in TestConfig
            testSet.Timer.ElapsedTime = 2.0f;

            // observer stops its task and long tap failed
            Assert.IsFalse(testSet.Enumerator.MoveNext());
            Assert.IsFalse(position.HasValue);
        }

        void ILongTapListener.OnLongTap(Vector2 position)
        {
            this.position = position;
        }
    }
}
