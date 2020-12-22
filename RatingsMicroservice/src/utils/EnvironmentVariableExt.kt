package hu.bme.aut.ratings.utils

fun getenvOrDefault(param: String, default: String): String {
    return System.getenv(param) ?: default
}