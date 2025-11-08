export const useAuth = () => {
    const token = useState<string | null>('token', () => null);
    const router = useRouter();

    const setToken = (t: string | null) => token.value = t;
    const isLogged = () => !!token.value;

    const fetchWithAuth = async (url: string, opts: any = {}) => {
        const config = useRuntimeConfig();
        opts.headers = opts.headers || { "Content-Type": "application/json"};
        if (token.value) opts.headers['Authorization'] = `Bearer ${token.value}`;

        try {
            const res: any = await $fetch(`${config.public.apiBase}${url}`, opts);
            return res;
        } catch (err: any) {
            if (err?.response?.status === 401 || err?.response?.status === 403) {
                token.value = null;
                router.push("/");
                return;
            }
            throw err;
        }
    };

    return { token, setToken, isLogged, fetchWithAuth };
};
