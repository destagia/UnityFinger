using NUnit.Framework;
using UnityFinger.ObserverFactories;
using UnityEngine;

namespace UnityFinger.Test
{
    class ScreenObserverTest : IScreenListener
    {
        ObserverTestSet<ScreenObserverFactory> testSet;

        Vector2? position;

        [SetUp]
        public void SetUp()
        {
            position = null;

            testSet = new ObserverTestSet<ScreenObserverFactory>();
            testSet.SetUp(() => new ScreenObserverFactory(this));
        }

        void IScreenListener.OnScreen(Vector2 position)
        {
            this.position = position;
        }

        [Test]
        public void Works()
        {
            testSet.Input.SetPosition(new Vector2(5, 5));

            Assert.AreEqual(new Vector2(5, 5), testSet.Input.GetPosition());

            Assert.IsFalse(testSet.Enumerator.MoveNext());
            Assert.AreEqual(Observation.None, testSet.Enumerator.Current);
            Assert.AreEqual(new Vector2(5, 5), position);
        }
    }
}
