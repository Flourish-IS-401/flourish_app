const AUTH_KEY = 'flourish_auth';

export const getAuth = () => {
    const stored = localStorage.getItem(AUTH_KEY);
    if (!stored) return null;
    try {
        return JSON.parse(stored);
    } catch {
        return null;
    }
};

/**
 * @param {object} [opts]
 * @param {string|null|undefined} [opts.accessToken] — JWT for Safari / strict browsers when cross-site cookies fail. Omit to keep previous token.
 */
export const setAuth = (userType, userId = null, opts = {}) => {
    const prev = getAuth() || {};
    const hasTokenOpt = Object.prototype.hasOwnProperty.call(opts, 'accessToken');
    const accessToken = hasTokenOpt ? opts.accessToken : prev.accessToken;
    const payload = { loggedIn: true, userType };
    if (userId != null && userId !== '') payload.userId = userId;
    if (accessToken) payload.accessToken = accessToken;
    localStorage.setItem(AUTH_KEY, JSON.stringify(payload));
};

export const getAccessToken = () => {
    const auth = getAuth();
    return auth?.accessToken ?? null;
};

export const clearAuth = () => {
    localStorage.removeItem(AUTH_KEY);
};

export const isLoggedIn = () => {
    const auth = getAuth();
    if (!auth) return false;
    return auth.loggedIn === true;
};

export const getUserType = () => {
    const auth = getAuth();
    if (!auth) return null;
    return auth.userType;
};

export const getUserId = () => {
    const auth = getAuth();
    if (!auth) return null;
    return auth.userId ?? auth.user_id ?? null;
};
