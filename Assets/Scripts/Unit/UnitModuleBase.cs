namespace CCCD
{
    public class UnitModuleBase
    {
        public Unit Unit { get; private set; }

        public UnitModuleBase(Unit unit)
        {
            Unit = unit;
        }
    }
}