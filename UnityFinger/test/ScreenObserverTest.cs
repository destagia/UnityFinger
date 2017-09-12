using NUnit.Framework;
using UnityFinger.Observers;
using UnityEngine;

namespace UnityFinger.Tests
{
    class ScreenObserverTest : IScreenListener
    {
        ObserverTestSet<ScreenObserver> testSet;

        Vector2? position;

        [SetUp]
        public void SetUp()
        {
            position = null;

            testSet = new ObserverTestSet<ScreenObserver>();
            testSet.SetUp(() => new ScreenObserver(this));
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
            Assert.AreEqual(Result.None, testSet.Enumerator.Current);
            Assert.AreEqual(new Vector2(5, 5), position);
        }
    }
}
