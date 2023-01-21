using MagicOnion;

namespace Eridu.Rpg {

    public interface ILivelinessService : IService<ILivelinessService> {
        UnaryResult<string> Status();
    }
}
