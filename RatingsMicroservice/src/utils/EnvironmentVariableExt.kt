package hu.bme.aut.ratings.utils

fun getenvCheckNotNull(param: String): String {
    return checkNotNull(System.getenv(param)) { "Environment variable '${param}' must be set." }
}