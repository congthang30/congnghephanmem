// Floating Chat Functionality
document.addEventListener('DOMContentLoaded', function() {
    const floatingChatBtn = document.getElementById('floating-chat-btn');
    const floatingChatWindow = document.getElementById('floating-chat-window');
    const closeChatBtn = document.getElementById('close-chat-btn');
    const messageInput = document.getElementById('floating-message-input');
    const chatArea = document.getElementById('floating-chat-area');

    // Toggle chat window
    floatingChatBtn.addEventListener('click', function() {
        floatingChatWindow.classList.toggle('show');
        if (floatingChatWindow.classList.contains('show')) {
            messageInput.focus();
        }
    });

    // Close chat window
    closeChatBtn.addEventListener('click', function() {
        floatingChatWindow.classList.remove('show');
    });

    // Close chat when clicking outside
    document.addEventListener('click', function(event) {
        if (!floatingChatWindow.contains(event.target) && 
            !floatingChatBtn.contains(event.target) && 
            floatingChatWindow.classList.contains('show')) {
            floatingChatWindow.classList.remove('show');
        }
    });

    // Send message with Enter key
    messageInput.addEventListener('keypress', function(event) {
        if (event.key === 'Enter' && !event.shiftKey) {
            event.preventDefault();
            sendFloatingMessage();
        }
    });

    // Auto-scroll to bottom when new message is added
    function scrollToBottom() {
        chatArea.scrollTop = chatArea.scrollHeight;
    }

    // Add message to floating chat
    function addFloatingMessage(text, type) {
        const messageDiv = document.createElement('div');
        messageDiv.className = `message ${type}`;
        
        const contentDiv = document.createElement('div');
        contentDiv.className = 'message-content';
        
        if (type === 'user') {
            contentDiv.innerHTML = `<strong>Bạn:</strong> ${text}`;
        } else if (type === 'ai') {
            contentDiv.innerHTML = `<strong>AI Assistant:</strong> ${text}`;
        } else if (type === 'error') {
            contentDiv.innerHTML = `<i class="fas fa-exclamation-triangle me-2"></i>${text}`;
        }
        
        messageDiv.appendChild(contentDiv);
        chatArea.appendChild(messageDiv);
        
        scrollToBottom();
    }

    // Send floating message
    window.sendFloatingMessage = async function() {
        const message = messageInput.value.trim();
        
        if (!message) {
            return;
        }

        // Disable input while processing
        messageInput.disabled = true;
        
        // Add user message
        addFloatingMessage(message, 'user');
        messageInput.value = '';

        try {
            const response = await fetch('/api/chatapi/send', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ userMessage: message })
            });
            
            const data = await response.json();

            if (!response.ok) {
                addFloatingMessage(`Lỗi: ${data.error || 'Có lỗi xảy ra'}`, 'error');
                return;
            }

            const reply = data.candidates?.[0]?.content?.parts?.[0]?.text || 'Không có phản hồi từ AI';
            addFloatingMessage(reply, 'ai');
            
        } catch (error) {
            addFloatingMessage(`Lỗi kết nối: ${error.message}`, 'error');
        } finally {
            // Re-enable input
            messageInput.disabled = false;
            messageInput.focus();
        }
    };

    // Show notification when chat is available
    setTimeout(function() {
        floatingChatBtn.style.animation = 'pulse 2s infinite';
    }, 2000);
}); 