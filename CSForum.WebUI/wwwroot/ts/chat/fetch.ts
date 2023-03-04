export const validation = (method: string, variables?: any) => {
    if ((method === "GET" || method == "DELETE")!=true) {
        return JSON.stringify({variables})
    }
}

export const request = async (pathUrl: string, method: "POST" | "GET" | "PUT" | "DELETE", headers?: any, variables?: any) => {
    return await fetch(pathUrl, {
        method: method,
        headers: headers,
        body: validation(method, variables)
    });
}