package hu.bme.aut.ratings.utils

import com.papsign.ktor.openapigen.OpenAPIGen
import com.papsign.ktor.openapigen.model.operation.OperationModel
import com.papsign.ktor.openapigen.modules.ModuleProvider
import com.papsign.ktor.openapigen.modules.RouteOpenAPIModule
import com.papsign.ktor.openapigen.modules.openapi.OperationModule

fun id(id: String? = null) = EndpointId(id)

data class EndpointId(val id: String? = null) : OperationModule, RouteOpenAPIModule {
    override fun configure(apiGen: OpenAPIGen, provider: ModuleProvider<*>, operation: OperationModel) {
        operation.operationId = id
    }
}