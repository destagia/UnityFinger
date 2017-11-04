using NUnit.Framework;
using UnityFinger.Factories;
using UnityEngine;
using System;

namespace UnityFinger.Test
{
    class PinchObserverTest : IPinchListener
    {
        PinchInfo? pinchStartInfo;
        PinchInfo? pinchInfo;
        bool pinchEnd;

        public void OnPinch(PinchInfo info)
        {
            pinchInfo = info;
        }

        public void OnPinchStart(PinchInfo info)
        {
            pinchStartInfo = info;
        }

        public void OnPinchEnd()
        {
            pinchEnd = true;
        }

        ObserverTestSet<PinchObserverFactory> testSet;

        [SetUp]
        public void SetUp()
        {
            pinchStartInfo = null;
            pinchInfo = null;
            pinchEnd = false;

            testSet = new ObserverTestSet<PinchObserverFactory>();
            testSet.SetUp(() => new PinchObserverFactory(new TestConfig(), this));
        }

        [Test]
        public void Works()
        {
            // simulate two fingers are on the screen
            testSet.Input.FingerCount = 2;
            testSet.Input.SetPosition(new Vector2(10f, 20f));
            testSet.Input.SetSecondPosition(new Vector2(30f, 40f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);
            Assert.IsFalse(pinchStartInfo.HasValue);

            // two fingers have been on the screen for the required distance
            // finger moves again and again
            for (var i = 1; i <= 10; i++) {
                var j = i - 1;
                testSet.Input.SetPosition(new Vector2(10f, 20f) + new Vector2(i, i));
                testSet.Input.SetSecondPosition(new Vector2(30f, 40f) + new Vector2(i, i));

                Assert.IsTrue(testSet.Enumerator.MoveNext());
                Assert.AreEqual(Observation.Fired, testSet.Enumerator.Current);

                PinchInfo? target = null;
                if (i == 1) {
                    target = pinchStartInfo;
                } else {
                    target = this.pinchInfo;
                }

                Assert.IsTrue(target.HasValue);
                Assert.AreEqual(new Vector2(10f, 20f) + new Vector2(i, i), target.Value.First.Current);
                Assert.AreEqual(new Vector2(10f, 20f) + new Vector2(j, j), target.Value.First.Previous);
                Assert.AreEqual(new Vector2(10f, 20f), target.Value.First.Origin);
                Assert.AreEqual(new Vector2(30f, 40f) + new Vector2(i, i), target.Value.Second.Current);
                Assert.AreEqual(new Vector2(30f, 40f) + new Vector2(j, j), target.Value.Second.Previous);
                Assert.AreEqual(new Vector2(30f, 40f), target.Value.Second.Origin);
            }

            testSet.Input.FingerCount = 0;

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.Fired, testSet.Enumerator.Current);
            Assert.IsTrue(pinchEnd);

            Assert.IsFalse(testSet.Enumerator.MoveNext());
        }

        [Test]
        public void FailsIfFingerDoesntMove()
        {
            // simulate two fingers are on the screen
            testSet.Input.FingerCount = 2;
            testSet.Input.SetPosition(new Vector2(10f, 20f));
            testSet.Input.SetSecondPosition(new Vector2(30f, 40f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);

            // It doesn't matter if time pasted
            testSet.Timer.ElapsedTime = 10f;

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);

            // It doesn't matter if finger moved so slightly
            testSet.Input.SetPosition(new Vector2(10.001f, 20.001f));
            testSet.Input.SetPosition(new Vector2(30.001f, 40.001f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);

            // It doesn't matter if one of the finger moved
            testSet.Input.SetPosition(new Vector2(1000f, 2000f));

            Assert.IsTrue(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);
        }

        [Test]
        public void FailsIfOverTwoFingers()
        {
            testSet.Input.FingerCount = 3;
            Assert.IsFalse(testSet.Enumerator.MoveNext());
        }
    }
}
