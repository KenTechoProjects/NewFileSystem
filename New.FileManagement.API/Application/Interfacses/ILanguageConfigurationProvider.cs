﻿
namespace Application.Interfacses
{
    public interface ILanguageConfigurationProvider
    {
        LanguagePack GetPack(string language);
    }
}
