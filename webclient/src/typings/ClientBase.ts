export class ClientBase {
    /**
     * authorization token value
     */
    public token: string | null;

    constructor() {
        this.token = null
    }

    protected transformOptions(options: any) {

        if (this.token) {
            options.headers["Authorization"] = "Bearer " + this.token
        } else {
            console.warn("Authorization token have not been set please authorize first.");
        }
        return Promise.resolve(options);
    }
}