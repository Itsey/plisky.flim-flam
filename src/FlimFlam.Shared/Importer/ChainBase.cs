namespace Plisky.Diagnostics.FlimFlam {

    public abstract class ChainBase<TRequest, TResponse> {
        protected ChainBase<TRequest, TResponse>? next = null;

        public virtual TResponse? Handle(TRequest source) {
            if (next != null) {
                return next.Handle(source);
            }
            return default(TResponse);
        }

        public ChainBase<TRequest, TResponse> Link(ChainBase<TRequest, TResponse> nxt) {
            next = nxt;
            return nxt;
        }
    }
}