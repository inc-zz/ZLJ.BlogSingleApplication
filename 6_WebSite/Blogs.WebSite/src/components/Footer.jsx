import React from 'react';
import { FaGithub, FaTwitter, FaEnvelope, FaPhone, FaWeixin } from 'react-icons/fa';

const Footer = () => {
  const currentYear = new Date().getFullYear();

  const friendlyLinks = [
    { name: 'React', url: 'https://react.dev' },
    { name: 'Vite', url: 'https://vitejs.dev' },
    { name: 'MDN Web Docs', url: 'https://developer.mozilla.org' },
    { name: 'GitHub', url: 'https://github.com' },
  ];

  const contactInfo = {
    email: '392090057@qq.com',
    phone: '+86 17302602302',
    wechat: 'zlj392090057'
  };

  return (
    <footer className="footer">
      <div className="container">
        <div className="footer-content">
          {/* 网站说明 */}
          <div className="footer-section">
            <h3 className="footer-title">关于本站</h3>
            <p className="footer-description">
              专注于技术分享与交流的博客平台，为企业提供架构落地解决方案、
              项目案例和学习资源。我们致力于构建一个开放、友好的技术社区。
            </p>
            <div className="footer-social">
              <a href="https://github.com/inc-zz" target="_blank" rel="noopener noreferrer" className="social-link">
                <FaGithub />
              </a>
              <a href="https://gitee.com/inc-zz" target="_blank" rel="noopener noreferrer" className="social-link">
                <FaTwitter />
              </a>
            </div>
          </div>

          {/* 友情链接 */}
          <div className="footer-section">
            <h3 className="footer-title">友情链接</h3>
            <ul className="footer-links">
              {friendlyLinks.map((link, index) => (
                <li key={index}>
                  <a href={link.url} target="_blank" rel="noopener noreferrer">
                    {link.name}
                  </a>
                </li>
              ))}
            </ul>
          </div>

          {/* 联系方式 */}
          <div className="footer-section">
            <h3 className="footer-title">联系我们</h3>
            <ul className="footer-contact">
              <li>
                <FaEnvelope className="contact-icon" />
                <a href={`mailto:${contactInfo.email}`}>{contactInfo.email}</a>
              </li>
              <li>
                <FaPhone className="contact-icon" />
                <span>{contactInfo.phone}</span>
              </li>
              <li>
                <FaWeixin className="contact-icon" />
                <span>{contactInfo.wechat}</span>
              </li>
            </ul>
          </div>
        </div>

        {/* 底部版权信息 */}
        <div className="footer-bottom">
          <p className="footer-copyright">
            © {currentYear} BlogSite. All rights reserved.
          </p>
          <p className="footer-icp">
            <a href="https://beian.miit.gov.cn" target="_blank" rel="noopener noreferrer">
              京ICP备12345678号-1
            </a>
          </p>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
