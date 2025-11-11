// 主题色彩系统配置
export const theme = {
  colors: {
    // 主色 - 淡灰蓝
    primary: {
      light: '#F5F7FA',
      main: '#E8ECF1',
      dark: '#D5DBE3',
    },
    // 辅色 - 深蓝
    secondary: {
      light: '#4A5F7F',
      main: '#2C3E50',
      dark: '#1A2332',
    },
    // 点缀色 - 橙黄
    accent: {
      light: '#FFB84D',
      main: '#FF9800',
      dark: '#E68900',
    },
    // 文本颜色
    text: {
      primary: '#2C3E50',
      secondary: '#546E7A',
      light: '#90A4AE',
      white: '#FFFFFF',
    },
    // 背景颜色
    background: {
      default: '#F5F7FA',
      paper: '#FFFFFF',
      grey: '#E8ECF1',
    },
    // 状态颜色
    status: {
      success: '#4CAF50',
      error: '#F44336',
      warning: '#FF9800',
      info: '#2196F3',
    },
    // 边框颜色
    border: {
      light: '#E0E0E0',
      main: '#BDBDBD',
      dark: '#9E9E9E',
    },
  },
  // 圆角
  borderRadius: {
    small: '4px',
    medium: '8px',
    large: '12px',
    xlarge: '16px',
  },
  // 阴影
  shadows: {
    small: '0 2px 4px rgba(0, 0, 0, 0.05)',
    medium: '0 4px 8px rgba(0, 0, 0, 0.08)',
    large: '0 8px 16px rgba(0, 0, 0, 0.12)',
    hover: '0 6px 12px rgba(0, 0, 0, 0.15)',
  },
  // 间距
  spacing: {
    xs: '4px',
    sm: '8px',
    md: '16px',
    lg: '24px',
    xl: '32px',
    xxl: '48px',
  },
  // 断点
  breakpoints: {
    mobile: '480px',
    tablet: '768px',
    desktop: '1024px',
    wide: '1280px',
  },
  // 字体
  typography: {
    fontFamily: '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif',
    fontSize: {
      xs: '12px',
      sm: '14px',
      md: '16px',
      lg: '18px',
      xl: '20px',
      xxl: '24px',
      xxxl: '32px',
    },
    fontWeight: {
      light: 300,
      regular: 400,
      medium: 500,
      semibold: 600,
      bold: 700,
    },
  },
  // 过渡动画
  transitions: {
    fast: '0.15s ease-in-out',
    normal: '0.3s ease-in-out',
    slow: '0.5s ease-in-out',
  },
};

export default theme;
