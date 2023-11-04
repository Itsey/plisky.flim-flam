namespace FlimFlam.Tests {
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using AutoFixture;
    using Plisky.Diagnostics.FlimFlam;
    using Support.ImportManagement;
    using Xunit;

    public class EventImportTests {

        private readonly Fixture fx;
        public EventImportTests() {
            fx = new Fixture();
        }

        [Fact(DisplayName = nameof(EventSubscriberWorksSingleSource))]
        public void EventSubscriberWorksSingleSource() {
            int hits = 0;

            var rax = fx.Create<RawApplicationEvent>();
            var inney = new Subject<RawApplicationEvent>();
            var sut = new EventImport();
            sut.ProvideEvents(inney);

            var outey = sut.Events.Subscribe(v => { hits++; });

            inney.OnNext(rax);
            inney.OnNext(rax);
            inney.OnNext(rax);

            Assert.Equal(3, hits);
        }


        [Fact(DisplayName = nameof(EventSubscriberCombinesSources))]
        public void EventSubscriberCombinesSources() {
            int hits = 0;

            var rax = fx.Create<RawApplicationEvent>();
            var srcs = new Subject<RawApplicationEvent>[3];

            var sut = new EventImport();
            for (int i = 0; i < srcs.Length; i++) {
                srcs[i] = new Subject<RawApplicationEvent>();
                sut.ProvideEvents(srcs[i]);
            }

            var outey = sut.Events.Subscribe(v => { hits++; });

            srcs[0].OnNext(rax);
            srcs[1].OnNext(rax);
            srcs[2].OnNext(rax);

            Assert.Equal(3, hits);
        }

    }
}
