using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using CustomPillows.Helpers;
using CustomPillows.Loaders;
using HMUI;
using SiraUtil.Logging;
using UnityEngine;
using Zenject;

namespace CustomPillows.Configuration.UI.Views
{
    [ViewDefinition("CustomPillows.Configuration.UI.Views.ImageListView")]
    [HotReload(RelativePathToLayout = @"ImageListView")]
    internal class ImageListView : BSMLAutomaticViewController
    {
        public event Action<IList<Texture2D>> OnSelectedTexturesChanged;
        public event Action OnSaveRequested;
        public event Action OnRefreshRequested;

        [UIComponent("image-list-left")] private readonly CustomListTableData _imageListLeft = null;
        [UIComponent("image-list-right")] private readonly CustomListTableData _imageListRight = null;

        private SiraLog _logger;
        private PluginConfig _config;
        private PillowImageLoader _pillowImageLoader;

        private IList<Texture2D> _leftTextures;
        private ISet<Texture2D> _rightTextures;

        private Texture2D _leftSelected;
        private Texture2D _rightSelected;

        [Inject]
        private void Construct(SiraLog logger, PluginConfig config, PillowImageLoader imageLoader)
        {
            _logger = logger;
            _pillowImageLoader = imageLoader;
            _config = config;
        }

        [UIAction("#post-parse")]
        private async void Setup()
        {
            await _pillowImageLoader.LoadAllAsync();

            _leftTextures = _pillowImageLoader.Images.Values.ToList();
            _rightTextures = new HashSet<Texture2D>();

            LoadSelectedTexturesFromConfig();

            RefreshLists();
        }

        private void LoadSelectedTexturesFromConfig()
        {
            var textures = _config.SelectedTextures.Split(';');

            foreach (var texture in textures)
            {
                if (_pillowImageLoader.TryGetImage(texture, out var tex)) _rightTextures.Add(tex);
            }
        }

        [UIAction("image-left-selected")]
        private void OnImageLeftSelected(TableView _, int idx)
        {
            _leftSelected = _leftTextures[idx];
        }

        [UIAction("image-right-selected")]
        private void OnImageRightSelected(TableView _, int idx)
        {
            _rightSelected = _rightTextures.ElementAt(idx);
        }

        [UIAction("click-add")]
        private void OnClickAdd()
        {
            if (_leftSelected == null) return;
            _rightTextures.Add(_leftSelected);
            RefreshRightList();

            OnSelectedTexturesChanged?.Invoke(_rightTextures.ToList());
        }

        [UIAction("click-remove")]
        private void OnClickRemove()
        {
            _rightTextures.Remove(_rightSelected);
            RefreshRightList();

            OnSelectedTexturesChanged?.Invoke(_rightTextures.ToList());
        }

        [UIAction("click-save")]
        private void OnClickSave()
        {
            OnSaveRequested?.Invoke();
        }

        [UIAction("click-shuffle")]
        private void OnClickShuffle()
        {
            OnRefreshRequested?.Invoke();
        }

        public void RefreshLists()
        {
            RefreshLeftList();
            RefreshRightList();
        }

        public void RefreshLeftList()
        {
            FillList(_imageListLeft, _leftTextures);
        }

        public void RefreshRightList()
        {
            FillList(_imageListRight, _rightTextures);
        }

        private void FillList(CustomListTableData list, IEnumerable<Texture2D> textures)
        {
            var cells = new List<CustomListTableData.CustomCellInfo>();
            foreach (var tex in textures)
            {
                var cell = new CustomListTableData.CustomCellInfo(tex.name, null, Utilities.LoadSpriteFromTexture(tex));
                cells.Add(cell);
            }

            list.data = cells;
            list.tableView.ReloadData();
        }
    }
}