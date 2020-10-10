package hu.bme.aut.ratings.utils

fun getenvCheckNotNull(param: String): String {
    return checkNotNull(System.getenv(param)) { "Environment variable '${param}' must be set." }
}

fun getenvOrDefault(param: String, default: String): String {
    return System.getenv(param) ?: default
}