﻿using Microsoft.Xna.Framework;
using static Gridly.Inputs;

namespace Gridly.UI
{
    public abstract class Clickable : GUI
    {
        /// <summary>
        /// 마우스가 처음 UI의 범위 안에 들어왔는가
        /// </summary>
        protected bool IsMouseEntered { get; private set; } = false;
        /// <summary>
        /// 마우스가 UI의 범위 위에서 움직였는가
        /// </summary>
        protected bool IsMouseMoved { get; private set; } = false;
        /// <summary>
        /// 마우스가 UI의 범위 안에 있는가
        /// </summary>
        protected bool IsMouseHover { get; private set; } = false;
        /// <summary>
        /// 마우스가 UI을 처음 눌렀는가
        /// </summary>
        protected bool IsMouseDown { get; private set; } = false;
        /// <summary>
        /// 마우스가 UI에서 뗐는가
        /// </summary>
        protected bool IsMouseUp { get; private set; } = false;
        /// <summary>
        /// 마우스가 막 UI의 범위를 벗어났는가
        /// </summary>
        protected bool IsMouseLeaved { get; private set; } = false;
        /// <summary>
        /// 마우스가 UI를 누르는 중인가
        /// </summary>
        protected bool IsMousePressing { get; private set; } = false;
        /// <summary>
        /// 마우스가 UI의 범위 안에서 스크롤
        /// </summary>
        protected int MouseWheel { get; private set; } = 0;

        private int _previouseWheel;
        private bool _previousMouseDown = false;
        private bool _shouldNotBeDown = false;

        public override bool ProcessInput()
        {
            if (IsMouseEntered) IsMouseEntered = false;
            if (IsMouseDown) IsMouseDown = false;
            if (IsMouseUp) IsMouseUp = false;
            if (IsMouseLeaved) IsMouseLeaved = false;

            if (IsHovering(MousePos))
            {
                if (!IsMouseHover)
                {
                    if (_previousMouseDown)
                        _shouldNotBeDown = true;
                    IsMouseHover = true;
                    IsMouseEntered = true;
                }
                if (IsLeftMousePressing())
                {
                    if (!IsMousePressing && !_shouldNotBeDown)
                    {
                        // 처음 클릭함
                        IsMousePressing = true;
                        IsMouseDown = true;
                    }
                }
                else
                {
                    if (IsMousePressing)
                    {
                        IsMousePressing = false;
                        IsMouseUp = true;
                    }
                    _shouldNotBeDown = false;
                }

                var scroll = ScrollWheelValue;
                MouseWheel = scroll - _previouseWheel;
                _previouseWheel = scroll;
            }
            else
            {
                if (IsMouseHover)
                {
                    // 막 벗어남
                    IsMouseLeaved = true;
                    IsMouseHover = false;
                    _shouldNotBeDown = false;
                }
                if (IsMousePressing)
                {
                    // 안에서 클릭해서 밖으로 드래그한 경우
                    if (!IsLeftMousePressing())
                    {
                        IsMousePressing = false;
                        IsMouseUp = true;
                        _shouldNotBeDown = false;
                    }
                }
            }

            _previousMouseDown = IsLeftMousePressing();

            return HandleInputs();
        }
        protected abstract bool HandleInputs();
    }
}
