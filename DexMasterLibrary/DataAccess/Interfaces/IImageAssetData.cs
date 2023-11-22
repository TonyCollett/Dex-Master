﻿namespace DexMasterLibrary.DataAccess.Interfaces;

public interface IImageAssetData
{
    Task<ImageAsset> GetImageAssetByIdAsync(string id);
    Task<ImageAsset> GetImageAssetByNameAsync(string name);
    Task SaveImageAssetAsync(ImageAsset imageAsset);
}