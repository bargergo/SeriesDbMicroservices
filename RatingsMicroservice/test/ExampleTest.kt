package hu.bme.aut.ratings

import io.ktor.http.*
import io.ktor.server.testing.*
import kotlin.test.Test
import kotlin.test.assertEquals

class ExampleTest {
    @Test
    fun passingTest() {
        withTestApplication({ module(testing = true) }) {
            assertEquals(1, 1)
        }
    }
    @Test
    fun failingTest() {
        withTestApplication({ module(testing = true) }) {
            assertEquals(1, 2)
        }
    }
}
