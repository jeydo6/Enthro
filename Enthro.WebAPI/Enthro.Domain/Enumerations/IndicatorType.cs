namespace Enthro.Domain.Enumerations
{
    public enum IndicatorType
    {
        // Неопределено
        Null = 0,

        // Рост-возраст
        HeightForAge = 1,

        // Масса тела-возраст
        WeightForAge = 2,

        // Масса тела-рост
        WeightForHeight = 3,

        // ИМТ-возраст
        BMIForAge = 4,

        // Скорость набора массы тела
        WeightVelocity = 11,

        // Скорость набора роста
        HeightVelocity = 12
    }
}
