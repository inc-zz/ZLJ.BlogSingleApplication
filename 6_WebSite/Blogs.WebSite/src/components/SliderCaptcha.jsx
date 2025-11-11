import React, { useState, useRef, useEffect } from 'react';

const SliderCaptcha = ({ onSuccess, onFail }) => {
  const [isDragging, setIsDragging] = useState(false);
  const [sliderLeft, setSliderLeft] = useState(0);
  const [puzzleX, setPuzzleX] = useState(0);
  const [puzzleY, setPuzzleY] = useState(0);
  const [isVerified, setIsVerified] = useState(false);
  const [isFailed, setIsFailed] = useState(false);
  const [backgroundImage, setBackgroundImage] = useState('');
  
  const canvasRef = useRef(null);
  const puzzleRef = useRef(null);
  const sliderRef = useRef(null);
  const containerRef = useRef(null);
  
  const canvasWidth = 300;
  const canvasHeight = 150;
  const puzzleSize = 50;
  const puzzleIndent = 10;

  // 生成随机背景图
  const generateBackground = () => {
    const colors = [
      'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
      'linear-gradient(135deg, #f093fb 0%, #f5576c 100%)',
      'linear-gradient(135deg, #4facfe 0%, #00f2fe 100%)',
      'linear-gradient(135deg, #43e97b 0%, #38f9d7 100%)',
      'linear-gradient(135deg, #fa709a 0%, #fee140 100%)',
    ];
    return colors[Math.floor(Math.random() * colors.length)];
  };

  // 初始化画布
  const initCanvas = () => {
    const canvas = canvasRef.current;
    const puzzle = puzzleRef.current;
    
    if (!canvas || !puzzle) {
      console.warn('Canvas elements not ready');
      return;
    }
    
    const ctx = canvas.getContext('2d');
    const puzzleCtx = puzzle.getContext('2d');
    
    if (!ctx || !puzzleCtx) {
      console.warn('Canvas context not available');
      return;
    }
    
    // 清空画布并重置状态
    ctx.clearRect(0, 0, canvasWidth, canvasHeight);
    puzzleCtx.clearRect(0, 0, puzzleSize + puzzleIndent, canvasHeight);
    
    // 重置composite操作
    ctx.globalCompositeOperation = 'source-over';
    puzzleCtx.globalCompositeOperation = 'source-over';
    
    // 随机位置
    const x = Math.floor(Math.random() * (canvasWidth - puzzleSize - 50)) + 50;
    const y = Math.floor(Math.random() * (canvasHeight - puzzleSize - 20)) + 10;
    
    setPuzzleX(x);
    setPuzzleY(y);
    
    // 设置背景
    const bg = generateBackground();
    setBackgroundImage(bg);
    
    // 绘制主画布背景
    const gradient = ctx.createLinearGradient(0, 0, canvasWidth, canvasHeight);
    if (bg.includes('gradient')) {
      // Parse gradient colors from the bg string
      const colors = bg.match(/#[0-9a-f]{6}/gi) || [];
      if (colors.length >= 2) {
        gradient.addColorStop(0, colors[0]);
        gradient.addColorStop(1, colors[1]);
      } else {
        gradient.addColorStop(0, '#667eea');
        gradient.addColorStop(1, '#764ba2');
      }
    } else {
      gradient.addColorStop(0, '#667eea');
      gradient.addColorStop(1, '#764ba2');
    }
    
    ctx.fillStyle = gradient;
    ctx.fillRect(0, 0, canvasWidth, canvasHeight);
    
    // 添加一些装饰图案
    drawPattern(ctx);
    
    // 保存当前状态
    ctx.save();
    
    // 绘制拼图缺口
    drawPuzzlePath(ctx, x, y);
    ctx.globalCompositeOperation = 'destination-out';
    ctx.fill();
    
    // 恢复状态
    ctx.restore();
    
    // 绘制拼图块背景
    const puzzleGradient = puzzleCtx.createLinearGradient(0, 0, puzzleSize + puzzleIndent, canvasHeight);
    if (bg.includes('gradient')) {
      const colors = bg.match(/#[0-9a-f]{6}/gi) || [];
      if (colors.length >= 2) {
        puzzleGradient.addColorStop(0, colors[0]);
        puzzleGradient.addColorStop(1, colors[1]);
      } else {
        puzzleGradient.addColorStop(0, '#667eea');
        puzzleGradient.addColorStop(1, '#764ba2');
      }
    } else {
      puzzleGradient.addColorStop(0, '#667eea');
      puzzleGradient.addColorStop(1, '#764ba2');
    }
    
    puzzleCtx.fillStyle = puzzleGradient;
    puzzleCtx.fillRect(0, 0, puzzleSize + puzzleIndent, canvasHeight);
    drawPattern(puzzleCtx, -x, -y);
    
    // 保存状态
    puzzleCtx.save();
    
    // 裁剪出拼图形状
    puzzleCtx.globalCompositeOperation = 'destination-in';
    drawPuzzlePath(puzzleCtx, 0, y);
    puzzleCtx.fill();
    
    // 恢复状态并绘制边框
    puzzleCtx.restore();
    puzzleCtx.globalCompositeOperation = 'source-over';
    puzzleCtx.strokeStyle = 'rgba(255,255,255,0.8)';
    puzzleCtx.lineWidth = 2;
    drawPuzzlePath(puzzleCtx, 0, y);
    puzzleCtx.stroke();
  };

  // 绘制装饰图案
  const drawPattern = (ctx, offsetX = 0, offsetY = 0) => {
    ctx.save();
    ctx.translate(offsetX, offsetY);
    
    // 绘制一些圆点
    for (let i = 0; i < 20; i++) {
      ctx.fillStyle = `rgba(255,255,255,${Math.random() * 0.3})`;
      ctx.beginPath();
      ctx.arc(
        Math.random() * canvasWidth,
        Math.random() * canvasHeight,
        Math.random() * 10 + 2,
        0,
        Math.PI * 2
      );
      ctx.fill();
    }
    
    ctx.restore();
  };

  // 绘制拼图路径
  const drawPuzzlePath = (ctx, x, y) => {
    ctx.beginPath();
    // 顶部
    ctx.moveTo(x, y);
    ctx.lineTo(x + puzzleSize / 2 - puzzleIndent, y);
    ctx.arc(x + puzzleSize / 2, y, puzzleIndent, Math.PI, 0, false);
    ctx.lineTo(x + puzzleSize, y);
    
    // 右侧
    ctx.lineTo(x + puzzleSize, y + puzzleSize / 2 - puzzleIndent);
    ctx.arc(x + puzzleSize, y + puzzleSize / 2, puzzleIndent, -Math.PI / 2, Math.PI / 2, false);
    ctx.lineTo(x + puzzleSize, y + puzzleSize);
    
    // 底部
    ctx.lineTo(x, y + puzzleSize);
    
    // 左侧
    ctx.lineTo(x, y);
    ctx.closePath();
  };

  // 开始拖动
  const handleMouseDown = (e) => {
    if (isVerified || isFailed) return;
    setIsDragging(true);
    e.preventDefault();
  };

  // 拖动中
  const handleMouseMove = (e) => {
    if (!isDragging || isVerified || isFailed) return;
    
    const container = containerRef.current;
    const rect = container.getBoundingClientRect();
    const maxDistance = canvasWidth - puzzleSize - puzzleIndent;
    
    let distance;
    if (e.type.includes('mouse')) {
      distance = e.clientX - rect.left - puzzleSize / 2;
    } else {
      distance = e.touches[0].clientX - rect.left - puzzleSize / 2;
    }
    
    distance = Math.max(0, Math.min(distance, maxDistance));
    setSliderLeft(distance);
  };

  // 结束拖动
  const handleMouseUp = () => {
    if (!isDragging || isVerified || isFailed) return;
    setIsDragging(false);
    
    // 验证位置
    const tolerance = 5; // 容差范围
    if (Math.abs(sliderLeft - puzzleX) < tolerance) {
      setIsVerified(true);
      onSuccess && onSuccess();
    } else {
      setIsFailed(true);
      setTimeout(() => {
        setIsFailed(false);
        setSliderLeft(0);
        initCanvas();
      }, 1000);
      onFail && onFail();
    }
  };

  // 重置
  const handleReset = () => {
    setSliderLeft(0);
    setIsVerified(false);
    setIsFailed(false);
    setIsDragging(false);
    initCanvas();
  };

  useEffect(() => {
    // Add a small delay to ensure DOM is ready
    const timer = setTimeout(() => {
      initCanvas();
    }, 100);
    
    return () => clearTimeout(timer);
  }, []);

  useEffect(() => {
    if (isDragging) {
      document.addEventListener('mousemove', handleMouseMove);
      document.addEventListener('mouseup', handleMouseUp);
      document.addEventListener('touchmove', handleMouseMove, { passive: false });
      document.addEventListener('touchend', handleMouseUp);
      
      return () => {
        document.removeEventListener('mousemove', handleMouseMove);
        document.removeEventListener('mouseup', handleMouseUp);
        document.removeEventListener('touchmove', handleMouseMove);
        document.removeEventListener('touchend', handleMouseUp);
      };
    }
  }, [isDragging, sliderLeft, puzzleX]);

  return (
    <div className="slider-captcha">
      <div className="captcha-canvas-container" ref={containerRef}>
        <canvas
          ref={canvasRef}
          width={canvasWidth}
          height={canvasHeight}
          className="captcha-canvas"
        />
        <canvas
          ref={puzzleRef}
          width={puzzleSize + puzzleIndent}
          height={canvasHeight}
          className="captcha-puzzle"
          style={{ left: `${sliderLeft}px` }}
        />
        <button className="captcha-refresh" onClick={handleReset} title="刷新">
          ↻
        </button>
      </div>
      
      <div className={`captcha-slider ${isVerified ? 'verified' : ''} ${isFailed ? 'failed' : ''}`}>
        <div className="slider-track">
          <div className="slider-fill" style={{ width: `${sliderLeft + puzzleSize}px` }} />
        </div>
        <div
          ref={sliderRef}
          className="slider-button"
          style={{ left: `${sliderLeft}px` }}
          onMouseDown={handleMouseDown}
          onTouchStart={handleMouseDown}
        >
          {isVerified ? '✓' : isFailed ? '✗' : '→'}
        </div>
        <div className="slider-text">
          {isVerified ? '验证成功' : isFailed ? '验证失败' : '拖动滑块完成拼图'}
        </div>
      </div>
    </div>
  );
};

export default SliderCaptcha;
