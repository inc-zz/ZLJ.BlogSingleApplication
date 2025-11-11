import React from 'react';

const TermsModal = ({ isOpen, onClose, type = 'terms' }) => {
  if (!isOpen) return null;

  const content = type === 'terms' ? termsContent : privacyContent;

  return (
    <div className="terms-modal-overlay" onClick={onClose}>
      <div className="terms-modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="terms-modal-header">
          <h2>{content.title}</h2>
          <button className="terms-close-btn" onClick={onClose}>×</button>
        </div>
        
        <div className="terms-modal-body">
          {content.sections.map((section, index) => (
            <div key={index} className="terms-section">
              <h3>{section.title}</h3>
              <div className="terms-text">
                {section.content.map((paragraph, pIndex) => (
                  <p key={pIndex}>{paragraph}</p>
                ))}
              </div>
            </div>
          ))}
          
          <div className="terms-footer">
            <p className="terms-update">最后更新时间：2025年10月20日</p>
          </div>
        </div>
        
        <div className="terms-modal-actions">
          <button className="btn-agree" onClick={onClose}>
            我已阅读并同意
          </button>
        </div>
      </div>
    </div>
  );
};

// 用户协议内容
const termsContent = {
  title: '知识分享平台用户协议',
  sections: [
    {
      title: '一、协议的接受',
      content: [
        '欢迎使用知识分享平台（以下简称"本平台"）。在使用本平台服务之前，请您仔细阅读本协议的全部内容。',
        '当您注册成为本平台用户或使用本平台服务时，即表示您已充分阅读、理解并接受本协议的全部条款。如果您不同意本协议的任何内容，请立即停止使用本平台服务。',
      ]
    },
    {
      title: '二、账号注册与使用',
      content: [
        '1. 您在注册账号时应当提供真实、准确、完整的个人资料，并保证及时更新您的资料。',
        '2. 您应当妥善保管您的账号和密码，不得将账号出借、转让或以其他方式提供给他人使用。',
        '3. 如发现账号被他人非法使用或存在安全漏洞，您应当立即通知本平台。',
        '4. 您承诺不会使用本平台进行任何违法违规活动。',
      ]
    },
    {
      title: '三、内容发布规范',
      content: [
        '1. 您在本平台发布的所有内容（包括但不限于文章、评论、图片等）应当遵守国家法律法规。',
        '2. 您发布的内容不得包含以下信息：',
        '   • 违反国家法律法规的信息',
        '   • 侵犯他人知识产权的信息',
        '   • 包含虚假、诈骗、恶意、侮辱、诽谤、淫秽等不良信息',
        '   • 其他违反公序良俗的信息',
        '3. 您应当对自己发布的内容承担全部法律责任。',
      ]
    },
    {
      title: '四、知识产权',
      content: [
        '1. 您在本平台发布的原创内容，著作权归您所有。',
        '2. 您同意授予本平台免费的、永久的、不可撤销的、全球范围内的许可使用权。',
        '3. 本平台尊重知识产权，如您发现侵权内容，请及时联系我们处理。',
      ]
    },
    {
      title: '五、隐私保护',
      content: [
        '1. 本平台重视用户隐私保护，详细内容请查看《隐私政策》。',
        '2. 本平台承诺不会将您的个人信息出售或出租给任何第三方。',
        '3. 本平台会采取合理的技术措施保护您的个人信息安全。',
      ]
    },
    {
      title: '六、免责声明',
      content: [
        '1. 本平台仅为用户提供信息发布和交流的平台，对用户发布的内容不承担审查义务。',
        '2. 因不可抗力或其他本平台无法控制的原因导致的服务中断或故障，本平台不承担责任。',
        '3. 用户因使用本平台服务产生的任何损失，本平台不承担赔偿责任。',
      ]
    },
    {
      title: '七、协议修改',
      content: [
        '本平台有权随时修改本协议条款，修改后的协议将在本平台公布。',
        '如您继续使用本平台服务，即视为您接受修改后的协议。',
      ]
    },
  ]
};

// 隐私政策内容
const privacyContent = {
  title: '隐私政策',
  sections: [
    {
      title: '一、信息收集',
      content: [
        '为了向您提供更好的服务，我们可能会收集以下信息：',
        '1. 您主动提供的信息：用户名、邮箱、头像等注册信息。',
        '2. 自动收集的信息：访问日志、IP地址、浏览器类型、设备信息等。',
        '3. 使用服务产生的信息：发布的文章、评论、点赞记录等。',
      ]
    },
    {
      title: '二、信息使用',
      content: [
        '我们收集的信息将用于以下目的：',
        '1. 提供、维护和改进我们的服务。',
        '2. 向您发送服务相关的通知和更新。',
        '3. 分析用户行为，优化产品功能。',
        '4. 防范和处理欺诈、滥用等违规行为。',
      ]
    },
    {
      title: '三、信息共享',
      content: [
        '我们承诺不会出售您的个人信息。只有在以下情况下，我们可能会共享您的信息：',
        '1. 获得您的明确同意。',
        '2. 法律法规或政府部门要求。',
        '3. 为维护本平台及其他用户的合法权益。',
      ]
    },
    {
      title: '四、信息安全',
      content: [
        '我们采取以下措施保护您的信息安全：',
        '1. 使用加密技术传输和存储敏感信息。',
        '2. 建立严格的信息访问权限控制机制。',
        '3. 定期进行安全审计和漏洞扫描。',
        '4. 及时响应和处理安全事件。',
      ]
    },
    {
      title: '五、用户权利',
      content: [
        '您对自己的个人信息享有以下权利：',
        '1. 查询和获取您的个人信息。',
        '2. 更正或补充您的个人信息。',
        '3. 删除您的个人信息或注销账号。',
        '4. 撤回同意授权。',
      ]
    },
    {
      title: '六、Cookie使用',
      content: [
        '我们使用Cookie和类似技术来：',
        '1. 记住您的登录状态。',
        '2. 分析网站流量和用户行为。',
        '3. 提供个性化的内容推荐。',
        '您可以通过浏览器设置管理Cookie。',
      ]
    },
    {
      title: '七、联系我们',
      content: [
        '如您对本隐私政策有任何疑问或建议，请通过以下方式联系我们：',
        '邮箱：privacy@techblog.com',
        '我们将在收到您的反馈后尽快回复。',
      ]
    },
  ]
};

export default TermsModal;
